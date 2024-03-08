using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using NetSchool.Web.Pages.Account.Models;
using NetSchool.Web.Entities.User;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;

namespace NetSchool.Web.Pages.Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationStateProvider _authProvider;

        public AccountService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authProvider)
        {
            _httpClientFactory = httpClientFactory;
            _authProvider = authProvider;
        }

        public async Task ChangePassword(ChangePasswordModel model)
        {
            var requestContent = JsonContent.Create(model);
            var httpClient = _httpClientFactory.CreateClient("delegatingClient");
            var response = await httpClient.PostAsync("v1/accounts/ChangePassword", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        public async Task ConfirmEmail(string userEmail, string code)
        {
            var requestContent = JsonContent.Create(new EmailConfirmModel { Email = userEmail, Code = code });
            var httpClient = _httpClientFactory.CreateClient("delegatingClient");
            var response = await httpClient.PostAsync("v1/accounts/ConfirmEmail", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        public async Task EditUserProfileAsync(EditProfileModel model)
        {
            var requestContent = JsonContent.Create(model);
            var httpClient = _httpClientFactory.CreateClient("delegatingClient");
            var response = await httpClient.PutAsync("v1/accounts/Edit", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        public async Task<UserAccountModel> Get(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("delegatingClient");
            var response = await httpClient.GetAsync($"v1/accounts?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }

            var data = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserAccountModel>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new UserAccountModel();

            return user;
        }

        public async Task<string> GetUserIdAsync()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "sub").Value;

            return userId;
        }

        public async Task SendEmailToChangePassword(string email)
        {
            var requestContent = JsonContent.Create(new ResetPasswordModel { Email = email });
            var httpClient = _httpClientFactory.CreateClient("delegatingClient");
            var response = await httpClient.PostAsync("v1/accounts/ResetPassword", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }

        public async Task Subscribe(Guid followingId)
        {
            await SubscriptionHandling(followingId, true);
        }

        public async Task Unsubscribe(Guid followingId)
        {
            await SubscriptionHandling(followingId, false);
        }

        private async Task SubscriptionHandling(Guid followingId, bool subscribe)
        {
            var userId = await GetUserIdAsync();
            var requestContent = JsonContent.Create(new SubscribeModel { UserId = new Guid(userId), FollowId = followingId });

            var url = "v1/accounts/" + (subscribe ? "Subscribe" : "Unsubscribe");
            var httpClient = _httpClientFactory.CreateClient("delegatingClient");
            var response = await httpClient.PostAsync(url, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
        }
    }
}
