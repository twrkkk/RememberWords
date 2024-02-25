using NetSchool.Services.EmailSender.Models;

namespace NetSchool.Services.EmailSender;

public interface IEmailSender
{
    Task SendEmailAsync(EmailModel message);
}
