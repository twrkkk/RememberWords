using NetSchool.Services.EmailSender.Models;

namespace NetSchool.Services.Actions;

public interface IAction
{
    Task SendEmailConfirmationAsync(EmailModel model);
    Task SendResetPasswordEmailAsync(EmailModel model);
    Task SendEmailForSubscribersAsync(EmailModel model);
}
