using NetSchool.Web.Pages.Auth.Models;
using System.Threading.Tasks;

namespace NetSchool.Web.Pages.Auth.Services;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
}