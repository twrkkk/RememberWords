namespace NetSchool.Services.Actions;

using NetSchool.Services.EmailSender.Models;
using NetSchool.Services.RabbitMq;
using System.Threading.Tasks;

public class Action : IAction
{
    private readonly IRabbitMq rabbitMq;

    public Action(IRabbitMq rabbitMq)
    {
        this.rabbitMq = rabbitMq;
    }

    public async Task SendEmailConfirmation(EmailModel model)
    {
        await rabbitMq.PushAsync(QueueNames.EMAIL_CONFIRMATION, model);
    }

    public async Task SendResetPasswordEmail(EmailModel model)
    {
        await rabbitMq.PushAsync(QueueNames.RESET_PASSWORD, model);
    }
}
