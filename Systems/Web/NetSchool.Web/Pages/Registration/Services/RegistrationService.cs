using NetSchool.Web.Pages.Registration.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NetSchool.Web.Pages.Registration.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RegistrationService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task Create(RegisterUserAccountModel model)
    {
        var requestContent = JsonContent.Create(model);
        try
        {
            var httpClient = _httpClientFactory.CreateClient("delegatingClient");
            var response = await httpClient.PostAsync("v1/accounts", requestContent);
        }
        catch
        {
            throw;
        }
    }
}
