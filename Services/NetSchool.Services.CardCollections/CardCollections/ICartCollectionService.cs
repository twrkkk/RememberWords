using NetSchool.Context.Entities;

namespace NetSchool.Services.CardCollections.CardCollections;

public interface ICartCollectionService
{
    Task<IEnumerable<CardCollectionModel>> GetAll();
    Task<CardCollectionModel> Get(Guid id);
    Task<CardCollectionModel> Create(CreateModel model);
    Task Update(Guid id, UpdateModel model);
    Task Delete(Guid id);
    Task SendEmailForSubscribers(User user, CardCollection newCollection);
}
