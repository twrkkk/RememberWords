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
using System.Runtime.CompilerServices;
using System.Web;

namespace NetSchool.Services.CardCollections.CardCollections;

public class CartCollectionService : ICartCollectionService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly IModelValidator<CreateModel> _createModelValidator;
    private readonly IModelValidator<UpdateModel> _updateModelValidator;
    private readonly IAction action;
    private readonly UserManager<User> userManager;

    public CartCollectionService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper, IModelValidator<CreateModel> createModelValidator, IModelValidator<UpdateModel> updateModelValidator, IAction action, UserManager<User> userManager)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _createModelValidator = createModelValidator;
        _updateModelValidator = updateModelValidator;
        this.action = action;
        this.userManager = userManager;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetAll()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collections = await context.CardCollections
            .Include(x => x.User)
            .Include(x => x.Cards)
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<CardCollectionModel>>(collections);

        return result;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetAllWithName(string name)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collections = await context.CardCollections
            .Include(x => x.User)
            .Include(x => x.Cards)
            .Where(x=>x.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<CardCollectionModel>>(collections);

        return result;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetPage(PageParameters parameters)
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

    public async Task<CardCollectionModel> Get(Guid id)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collection = await context.CardCollections
            .Include(x => x.User)
            .Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Uid == id);

        if (collection == null)
            throw new EntityNotFoundException($"Collection (ID = {id}) not found.");

        var result = _mapper.Map<CardCollectionModel>(collection);

        return result;
    }

    public async Task<CardCollectionModel> Create(CreateModel model)
    {
        await _createModelValidator.CheckAsync(model);

        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collection = _mapper.Map<CardCollection>(model);

        var user = context.Users.Include(x => x.CardCollections).FirstOrDefault(x => x.Id == model.UserId);
        if (user != null)
        {
            collection.TimeExpiration = DateTime.UtcNow.AddDays(7);
            user.CardCollections.Add(collection);
        }

        await context.SaveChangesAsync();

        await SendEmailForSubscribers(user, collection);

        return _mapper.Map<CardCollectionModel>(collection);
    }

    public async Task Delete(Guid id)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var collection = await context.CardCollections.FirstOrDefaultAsync(x => x.Uid == id);

        if (collection == null)
            throw new EntityNotFoundException($"Collection (ID = {id}) not found.");

        context.CardCollections.Remove(collection);

        await context.SaveChangesAsync();
    }

    public async Task Update(Guid id, UpdateModel model)
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

        collection.Name = model.Name;

        context.CardCollections.Update(collection);

        await context.SaveChangesAsync();
    }

    public async Task SendEmailForSubscribers(User user, CardCollection newCollection)
    {
        var uriBuilder = new UriBuilder("https", "localhost", 7165, "/show-collection");
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

            await action.SendEmailForSubscribers(email);
        }
    }
}
