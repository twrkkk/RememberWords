using NetSchool.Web.Pages.Registration.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NetSchool.Web.Pages.Registration.Services;

public class RegistrationService : IRegistrationService
{
    private readonly HttpClient _httpClient;

    public RegistrationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Create(RegisterUserAccountModel model)
    {
        var requestContent = JsonContent.Create(model);
        try
        {
            var response = await _httpClient.PostAsync("v1/accounts", requestContent);
        }
        catch
        {
            throw;
        }
    }
}
