using NetSchool.Web.Pages.Registration.Models;
using System.Threading.Tasks;

namespace NetSchool.Web.Pages.Registration.Services;

public interface IRegistrationService
{
    Task Create(RegisterUserAccountModel model);
}
