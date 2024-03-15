using NetSchool.Context.Entities;
using NetSchool.Services.CardCollections.Models;

namespace NetSchool.Services.CardCollections.CardCollections;

public interface ICartCollectionService
{
    Task<IEnumerable<CardCollectionModel>> GetAllAsync();
    Task<IEnumerable<CardCollectionModel>> GetAllWithNameAsync(string name);
    Task<CardCollectionModel> GetAsync(Guid id);
    Task<IEnumerable<CardCollectionModel>> GetPageAsync(PageParameters parameters);
    Task<CardCollectionModel> CreateAsync(CreateModel model);
    Task UpdateAsync(Guid id, UpdateModel model);
    Task DeleteAsync(Guid id);
    Task SendEmailForSubscribersAsync(User user, CardCollection newCollection);
}
