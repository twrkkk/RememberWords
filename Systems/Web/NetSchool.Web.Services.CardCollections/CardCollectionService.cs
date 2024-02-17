using NetSchool.Web.Entities.CardCollections;
using System.Net.Http.Json;

namespace NetSchool.Web.Services.CardCollections;

public class CardCollectionService : ICardCollectionsService
{
    private readonly HttpClient _httpClient;

    public CardCollectionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CardCollectionModel>> GetAll()
    {
        var response = await _httpClient.GetAsync("v1/cardCollections");
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<CardCollectionModel>>() ?? new List<CardCollectionModel>();
    }

    public async Task<CardCollectionModel> Get(Guid id)
    {
        var response = await _httpClient.GetAsync($"v1/cardCollections/{id}");
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
        var response = await _httpClient.PostAsync("v1/cardCollections", requestContent);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task Update(Guid id, UpdateModel model)
    {
        var requestContent = JsonContent.Create(model);
        var response = await _httpClient.PutAsync($"v1/cardCollections/{id}", requestContent);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }
    public async Task Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"v1/cardCollections/{id}");

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }
}
