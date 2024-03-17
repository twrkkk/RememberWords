using NetSchool.Web.Entities.User;
using NetSchool.Web.Pages.Account.Models;
using System;
using System.Threading.Tasks;

namespace NetSchool.Web.Pages.Account.Services;

public interface IAccountService
{
    Task ConfirmEmailAsync(string email, string code);
    Task ChangePasswordAsync(ChangePasswordModel model);
    Task SendEmailToChangePasswordAsync(string email);
    Task SendEmailConfirmationAsync();
    Task<UserAccountModel> GetAsync(Guid id);
    Task<string> GetUserIdAsync();
    Task EditUserProfileAsync(EditProfileModel model);
    Task SubscribeAsync(Guid followingId);
    Task UnsubscribeAsync(Guid followingId);
}
