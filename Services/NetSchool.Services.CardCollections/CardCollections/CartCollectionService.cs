using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetSchool.Common.Exceptions;
using NetSchool.Common.Validator;
using NetSchool.Context;
using NetSchool.Context.Entities;
using NetSchool.Services.Actions;
using NetSchool.Services.CardCollections.Models;
using NetSchool.Services.EmailSender.Models;
using System.Web;
using Microsoft.Extensions.Caching.Distributed;
using NetSchool.Services.Logger;
using Newtonsoft.Json;
using NetSchool.Services.CardCollections.Cards.Models;
using FluentValidation;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using NetSchool.Services.CardCollections.CardCollections.Models;

namespace NetSchool.Services.CardCollections.CardCollections;

public class CartCollectionService : ICartCollectionService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly IModelValidator<CreateModel> _createModelValidator;
    private readonly IModelValidator<UpdateModel> _updateModelValidator;
    private readonly IAction action;
    private readonly UserManager<User> userManager;
    private readonly PdfGenerator.PdfGenerator pdfGenerator;
    private readonly IDistributedCache cache;
    private readonly IAppLogger logger;
    private readonly IHttpClientFactory _httpClientFactory;


    public CartCollectionService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper, IModelValidator<CreateModel> createModelValidator, IModelValidator<UpdateModel> updateModelValidator, IAction action, UserManager<User> userManager, PdfGenerator.PdfGenerator pdfGenerator, IDistributedCache cache, IHttpClientFactory httpClientFactory, IAppLogger logger)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _createModelValidator = createModelValidator;
        _updateModelValidator = updateModelValidator;
        this.action = action;
        this.userManager = userManager;
        this.pdfGenerator = pdfGenerator;
        this.cache = cache;
        _httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetAllAsync()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collections = await context.CardCollections
            .Include(x => x.User)
            .Include(x => x.Cards)
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<CardCollectionModel>>(collections);

        return result;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetAllWithNameAsync(string name)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collections = await context.CardCollections
            .Include(x => x.User)
            .Include(x => x.Cards)
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<CardCollectionModel>>(collections);

        return result;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetPageAsync(PageParameters parameters)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collections = await context.CardCollections
            .Include(x => x.User)
            .Include(x => x.Cards)
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<CardCollectionModel>>(collections);

        return result;
    }

    public async Task<CardCollectionModel> GetAsync(Guid id)
    {
        var cardCollectionString = await cache.GetStringAsync(id.ToString());
        CardCollectionModel result;

        if (cardCollectionString != null)
        {
            result = JsonConvert.DeserializeObject<CardCollectionModel>(cardCollectionString);
            logger.Information($"Collection Id: {id} was extracted from cache");
        }
        else
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();

            var cardCollection = await context.CardCollections
                .Include(x => x.User)
                .Include(x => x.Cards)
                .FirstOrDefaultAsync(x => x.Uid == id);

            if (cardCollection == null)
                throw new EntityNotFoundException($"Collection (ID = {id}) not found.");

            logger.Information($"Collection Id: {id} was extracted from DB");

            result = _mapper.Map<CardCollectionModel>(cardCollection);

            await SaveCardCollectionInCache(result);
        }

        return result;
    }

    public async Task<CardCollectionModel> CreateAsync(CreateModel model)
    {
        await _createModelValidator.CheckAsync(model);

        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collection = _mapper.Map<CardCollection>(model);

        var user = context.Users.Include(x => x.CardCollections).FirstOrDefault(x => x.Id == model.UserId);
        if (user != null)
        {
            collection.TimeExpiration = DateTime.UtcNow.AddDays((int)model.SavePeriod);
            user.CardCollections.Add(collection);
        }

        await context.SaveChangesAsync();

        var result = _mapper.Map<CardCollectionModel>(collection);
        await SaveCardCollectionInCache(result);

        await SendEmailForSubscribersAsync(user, collection);

        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collection = await context.CardCollections.FirstOrDefaultAsync(x => x.Uid == id);

        if (collection == null)
            throw new EntityNotFoundException($"Collection (ID = {id}) not found.");

        context.CardCollections.Remove(collection);

        await RemoveCardCollectionFromCache(id);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateModel model)
    {
        await _updateModelValidator.CheckAsync(model);

        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collection = await context.CardCollections.FirstOrDefaultAsync(x => x.Uid == id);

        if (collection == null)
            throw new EntityNotFoundException($"Collection (ID = {id}) not found.");

        foreach (var cardToUpdate in model.UpdatedCards)
        {
            var card = collection.Cards.FirstOrDefault(x => x.Uid == cardToUpdate.Id);
            if (card != null) // update exist card
            {
                card.Front = cardToUpdate.Front;
                card.Reverse = cardToUpdate.Reverse;
            }
            else //create new card
            {
                var newCard = _mapper.Map<Card>(cardToUpdate);
                collection.Cards.Add(newCard);
            }
        }

        foreach (var deleteCardId in model.DeletedCardsId)
        {
            var card = collection.Cards.FirstOrDefault(x => x.Uid == deleteCardId);

            if (card != null)
            {
                collection.Cards.Remove(card);
            }
        }

        collection.TimeExpiration = DateTime.UtcNow.AddDays((int)model.SavePeriod);
        collection.Name = model.Name;

        context.CardCollections.Update(collection);

        var result = _mapper.Map<CardCollectionModel>(collection);
        await SaveCardCollectionInCache(result);

        await context.SaveChangesAsync();
    }

    public async Task SendEmailForSubscribersAsync(User user, CardCollection newCollection)
    {
        var uriBuilder = new UriBuilder("http", "localhost", 7165, "/show-collection");
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["collectionId"] = newCollection.Uid.ToString();
        uriBuilder.Query = query.ToString();

        var callbackUrl = uriBuilder.ToString();

        foreach (var follower in user.Followers)
        {
            var email = new EmailModel
            {
                To = follower.Email,
                Subject = "Memorizing New Collection",
                Content = string.Format("{0} created a new card collection on Memorizing, to show it click <a href='{1}'>here</a>", user.UserName, callbackUrl)
            };

            await action.SendEmailForSubscribersAsync(email);
        }
    }

    public async Task<byte[]> CardCollectionToPdfAsync(Guid collectionId)
    {
        var collection = await GetAsync(collectionId);
        var result = _mapper.Map<CardCollection>(collection);

        return pdfGenerator.CardCollectionToPdf(result);
    }

    private async Task SaveCardCollectionInCache(CardCollectionModel cardCollection)
    {
        var cardCollectionString = JsonConvert.SerializeObject(cardCollection, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

        await cache.SetStringAsync(cardCollection.Id.ToString(), cardCollectionString, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
    }

    private async Task RemoveCardCollectionFromCache(Guid id)
    {
        await cache.RemoveAsync(id.ToString());
    }

    public async Task<IEnumerable<CreateCardModel>> GenerateWithAI(string prompt)
    {
        var generatedMessages = new[]
            {
                new
                {
                    role = "user",
                    text = $"Сгенерируй набор из 10 элементов такого вида [{{\"Front\": \"term\", \"Reverse\": \"definition\"}}] на тему: {prompt}"
                }
            };
        var requestContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
        {
            modelUri = "gpt://b1ghqukph951bqa0oejt/yandexgpt/rc",
            completionOptions = new { maxTokens = 600, temperature = 1 },
            messages = generatedMessages
        }), Encoding.UTF8, "application/json");

        var client = _httpClientFactory.CreateClient("YandexGPT");
        var response = await client.PostAsync("/foundationModels/v1/completion", requestContent);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        logger.Information(content);

        var gptResponse = System.Text.Json.JsonSerializer.Deserialize<GenerateCollectionResponse>(content);
        var rawJson = gptResponse?.result?.alternatives?.FirstOrDefault()?.message?.text;

        rawJson = rawJson.Trim('`');

        var cards = System.Text.Json.JsonSerializer.Deserialize<List<CreateCardModel>>(rawJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return cards;
    }
}
