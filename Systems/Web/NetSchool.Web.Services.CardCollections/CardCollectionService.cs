using NetSchool.Web.Entities.CardCollections;
using System.Net.Http.Json;

namespace NetSchool.Web.Services.CardCollections;

public class CardCollectionService : ICardCollectionsService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CardCollectionService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetAll()
    {
        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        var response = await httpClient.GetAsync("v1/cardCollections");
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<CardCollectionModel>>() ?? new List<CardCollectionModel>();
    }

    public async Task<IEnumerable<CardCollectionModel>> GetAllWithName(string name)
    {
        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        var response = await httpClient.GetAsync($"v1/cardCollections/{name}");
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<CardCollectionModel>>() ?? new List<CardCollectionModel>();
    }

    public async Task<IEnumerable<CardCollectionModel>> GetPage(int page, int pageSize)
    {
        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        var response = await httpClient.GetAsync($"v1/cardCollections/{page}/{pageSize}");
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<CardCollectionModel>>() ?? new List<CardCollectionModel>();
    }

    public async Task<CardCollectionModel> Get(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        var response = await httpClient.GetAsync($"v1/cardCollections/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }

        return await response.Content.ReadFromJsonAsync<CardCollectionModel>() ?? new CardCollectionModel();
    }

    public async Task Create(CreateModel model)
    {
        var requestContent = JsonContent.Create(model);
        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        var response = await httpClient.PostAsync("v1/cardCollections", requestContent);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task Update(Guid id, UpdateModel model)
    {
        var requestContent = JsonContent.Create(model);
        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        var response = await httpClient.PutAsync($"v1/cardCollections/{id}", requestContent);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task Delete(Guid id)
    {
        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        var response = await httpClient.DeleteAsync($"v1/cardCollections/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }
}
