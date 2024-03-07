using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Blazored.LocalStorage;
using NetSchool.Web.Pages.Auth.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace NetSchool.Web.DelegatingHandlers;

public class RefreshTokenDelegatingHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly NavigationManager _navManager;

    public RefreshTokenDelegatingHandler(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory, NavigationManager navManager)
    {
        _localStorage = localStorage;
        _httpClientFactory = httpClientFactory;
        _navManager = navManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _localStorage.GetItemAsync<string>(Constants.LocalStorageAuthTokenKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var content = await RefreshAccessToken();

        var loginResult = JsonSerializer.Deserialize<LoginResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LoginResult();

        if(loginResult.Error != null)
        {
            _navManager.NavigateTo("/login");
            return new HttpResponseMessage(HttpStatusCode.Redirect);
        }

        await _localStorage.SetItemAsync(Constants.LocalStorageAuthTokenKey, loginResult.AccessToken);
        await _localStorage.SetItemAsync(Constants.LocalStorageRefreshTokenKey, loginResult.RefreshToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);

        var responceWithNewAccessToken = await base.SendAsync(request, cancellationToken);

        return responceWithNewAccessToken;
    }

    private async Task<string> RefreshAccessToken()
    {
        var refreshToken = await _localStorage.GetItemAsync<string>(Constants.LocalStorageRefreshTokenKey);

        var url = $"{Settings.IdentityRoot}/connect/token";

        var request_body = new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("client_id", Settings.ClientId),
            new KeyValuePair<string, string>("client_secret", Settings.ClientSecret),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
        };

        var requestContent = new FormUrlEncodedContent(request_body);

        var httpClient = _httpClientFactory.CreateClient();

        var refreshTokenResponse = await httpClient.PostAsync(url, requestContent);

        var content = await refreshTokenResponse.Content.ReadAsStringAsync();

        return content;
    }
}
