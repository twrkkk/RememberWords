using System.Net.Http.Json;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using NetSchool.Web.Pages.Account.Models;

namespace NetSchool.Web.Pages.Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task ChangePassword(ChangePasswordModel model)
        {
            var requestContent = JsonContent.Create(model);
            try
            {
                var httpClient = _httpClientFactory.CreateClient("delegatingClient");
                var response = await httpClient.PostAsync("v1/accounts/ChangePassword", requestContent);
            }
            catch
            {
                throw;
            }
        }

        public async Task ConfirmEmail(string userEmail, string code)
        {
            var requestContent = JsonContent.Create(new EmailConfirmModel { Email = userEmail, Code = code});
            try
            {
                var httpClient = _httpClientFactory.CreateClient("delegatingClient");
                var response = await httpClient.PostAsync("v1/accounts/ConfirmEmail", requestContent);
            }
            catch
            {
                throw;
            }
        }

        public async Task SendEmailToChangePassword(string email)
        {
            var requestContent = JsonContent.Create(new ResetPasswordModel { Email = email });
            try
            {
                var httpClient = _httpClientFactory.CreateClient("delegatingClient");
                var response = await httpClient.PostAsync("v1/accounts/ResetPassword", requestContent);
            }
            catch
            {
                throw;
            }
        }
    }
}
