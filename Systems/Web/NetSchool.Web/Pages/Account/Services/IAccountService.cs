using NetSchool.Web.Entities.User;
using NetSchool.Web.Pages.Account.Models;
using System;
using System.Threading.Tasks;

namespace NetSchool.Web.Pages.Account.Services;

public interface IAccountService
{
    Task ConfirmEmail(string email, string code);
    Task ChangePassword(ChangePasswordModel model);
    Task SendEmailToChangePassword(string email);
    Task<UserAccountModel> Get(Guid id);
    Task<string> GetUserIdAsync();
    Task EditUserProfileAsync(EditProfileModel model);
}
