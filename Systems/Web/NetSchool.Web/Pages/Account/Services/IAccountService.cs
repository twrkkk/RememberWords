using System.Threading.Tasks;

namespace NetSchool.Web.Pages.Account.Services;

public interface IAccountService
{
    Task ConfirmEmail(string email, string code);
}
