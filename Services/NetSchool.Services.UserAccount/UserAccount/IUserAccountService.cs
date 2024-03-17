using NetSchool.Context.Entities;
using NetSchool.Services.UserAccount.Models;
using NetSchool.Services.UserAccount.UserAccount.Models;

namespace NetSchool.Services.UserAccount;

public interface IUserAccountService
{
    Task<bool> IsEmptyAsync();
    Task<UserAccountModel> CreateAsync(RegisterUserAccountModel model);
    Task SendEmailConfirmationAsync(User user);
    Task SendEmailConfirmationAsync(UserIdModel model);
    Task ConfirmEmailAsync(EmailConfirmModel model);
    Task SendEmailToChangePasswordAsync(ResetPasswordModel model);
    Task ChangePasswordAsync(ChangePasswordModel model);
    Task<UserAccountModel> GetAsync(Guid id);
    Task EditUserProfileAsync(EditProfileModel model);
    Task SubscribeAsync(SubscribeModel model);
    Task UnsubscribeAsync(SubscribeModel model);
}
