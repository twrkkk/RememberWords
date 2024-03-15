using NetSchool.Context.Entities;
using NetSchool.Services.CardCollections.Models;

namespace NetSchool.Services.CardCollections.CardCollections;

public interface ICartCollectionService
{
    Task<IEnumerable<CardCollectionModel>> GetAll();
    Task<IEnumerable<CardCollectionModel>> GetAllWithName(string name);
    Task<CardCollectionModel> Get(Guid id);
    Task<IEnumerable<CardCollectionModel>> GetPage(PageParameters parameters);
    Task<CardCollectionModel> Create(CreateModel model);
    Task Update(Guid id, UpdateModel model);
    Task Delete(Guid id);
    Task SendEmailForSubscribers(User user, CardCollection newCollection);
}
