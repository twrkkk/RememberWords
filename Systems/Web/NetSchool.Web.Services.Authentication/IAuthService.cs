using NetSchool.Web.Pages.Auth.Models;
using System.Threading.Tasks;

namespace NetSchool.Web.Services.Authentication;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
}