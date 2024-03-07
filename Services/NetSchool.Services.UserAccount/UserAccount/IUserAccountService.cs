using NetSchool.Context.Entities;
using NetSchool.Services.UserAccount.Models;
using NetSchool.Services.UserAccount.UserAccount.Models;

namespace NetSchool.Services.UserAccount;

public interface IUserAccountService
{
    Task<bool> IsEmpty();

    /// <summary>
    /// Create user account
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserAccountModel> Create(RegisterUserAccountModel model);
    Task SendEmailConfirmation(User user);
    Task ConfirmEmail(EmailConfirmModel model);
    Task SendEmailToChangePassword(ResetPasswordModel model);
    Task ChangePassword(ChangePasswordModel model);
    Task<UserAccountModel> Get(Guid id);
    Task EditUserProfileAsync(EditProfileModel model);



    // .. Также здесь можно разместить методы для изменения данных учетной записи, восстановления и смены пароля, подтверждения электронной почты, установки телефона и его подтверждения и т.д.
    // .. Но это уже на самостоятельно.
    // .. Удачи! Я в вас верю!  :)
}
