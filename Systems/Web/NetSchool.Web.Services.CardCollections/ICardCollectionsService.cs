using NetSchool.Web.Entities.CardCollections;

namespace NetSchool.Web.Services.CardCollections;

public interface ICardCollectionsService
{
    Task<IEnumerable<CardCollectionModel>> GetAll();
    Task<IEnumerable<CardCollectionModel>> GetAllWithName(string name);
    Task<IEnumerable<CardCollectionModel>> GetPage(int page, int pageSize);
    Task<CardCollectionModel> Get(Guid id);
    Task Create(CreateModel model);
    Task Update(Guid id, UpdateModel model);
    Task Delete(Guid id);
}
