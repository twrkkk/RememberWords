using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using NetSchool.Web.Pages.Auth.Models;
using NetSchool.Web.Providers;

namespace NetSchool.Web.Pages.Auth.Services;

public class AuthService : IAuthService
{    
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(
                       AuthenticationStateProvider authenticationStateProvider,
                       ILocalStorageService localStorage,
                       IHttpClientFactory httpClientFactory)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        var url = $"{Settings.IdentityRoot}/connect/token";

        var request_body = new[] 
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", Settings.ClientId),
            new KeyValuePair<string, string>("client_secret", Settings.ClientSecret),
            new KeyValuePair<string, string>("username", loginModel.Username!),
            new KeyValuePair<string, string>("password", loginModel.Password!)
        };

        var requestContent = new FormUrlEncodedContent(request_body);

        var httpClient = _httpClientFactory.CreateClient("delegatingClient");

        var response = await httpClient.PostAsync(url, requestContent);

        var content = await response.Content.ReadAsStringAsync();

        var loginResult = JsonSerializer.Deserialize<LoginResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LoginResult();
        loginResult.Successful = response.IsSuccessStatusCode;

        if (!response.IsSuccessStatusCode)
        {
            return loginResult;
        }

        await _localStorage.SetItemAsync(Constants.LocalStorageAuthTokenKey, loginResult.AccessToken);
        await _localStorage.SetItemAsync(Constants.LocalStorageRefreshTokenKey, loginResult.RefreshToken);

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Username!);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync(Constants.LocalStorageAuthTokenKey);
        await _localStorage.RemoveItemAsync(Constants.LocalStorageRefreshTokenKey);

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

        var httpClient = _httpClientFactory.CreateClient("delegatingClient");
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}