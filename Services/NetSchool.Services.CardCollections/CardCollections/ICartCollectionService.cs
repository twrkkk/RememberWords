namespace NetSchool.Services.CardCollections.CardCollections;

public interface ICartCollectionService
{
    Task<IEnumerable<CardCollectionModel>> GetAll();
    Task<CardCollectionModel> GetById(Guid id);
    Task<CardCollectionModel> Create(CreateModel model);
    Task Update(Guid id, UpdateModel model);
    Task Delete(Guid id);
}
