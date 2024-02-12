using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NetSchool.Common.Exceptions;
using NetSchool.Common.Validator;
using NetSchool.Context;
using NetSchool.Context.Entities;

namespace NetSchool.Services.CardCollections.CardCollections;

public class CartCollectionService : ICartCollectionService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly IModelValidator<CreateModel> _createModelValidator;
    private readonly IModelValidator<UpdateModel> _updateModelValidator;

    public CartCollectionService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper, IModelValidator<CreateModel> createModelValidator, IModelValidator<UpdateModel> updateModelValidator)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _createModelValidator = createModelValidator;
        _updateModelValidator = updateModelValidator;
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

        var user = context.Users.Include(x => x.CardCollections).FirstOrDefault(x => x.Uid == model.UserId);
        if (user != null)
        {
            user.CardCollections.Add(collection);
        }

        await context.SaveChangesAsync();

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

        foreach (var cardToUpdate in model.Cards)
        {
            var card = collection.Cards.FirstOrDefault(x => x.Uid == cardToUpdate.Id);
            if (card != null)
            {
                card.Front = cardToUpdate.Front;
                card.Reverse = cardToUpdate.Reverse;
            }
        }

        collection.Name = model.Name;

        context.CardCollections.Update(collection);

        await context.SaveChangesAsync();
    }
}
