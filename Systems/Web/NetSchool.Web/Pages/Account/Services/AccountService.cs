using System.Net.Http.Json;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using NetSchool.Web.Pages.Account.Models;

namespace NetSchool.Web.Pages.Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ConfirmEmail(string userEmail, string code)
        {
            var requestContent = JsonContent.Create(new EmailConfirmModel { Email = userEmail, Code = code});
            try
            {
                var response = await _httpClient.PostAsync("v1/accounts/ConfirmEmail", requestContent);
            }
            catch
            {
                throw;
            }
        }
    }
}
