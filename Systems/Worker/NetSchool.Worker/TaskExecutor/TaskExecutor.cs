namespace NetSchool.Worker;

using NetSchool.Services.RabbitMq;
using NetSchool.Services.Actions;
using NetSchool.Services.Logger;
using NetSchool.Services.EmailSender.Models;
using NetSchool.Services.EmailSender;

public class TaskExecutor : ITaskExecutor
{
    private readonly IAppLogger logger;
    private readonly IRabbitMq rabbitMq;
    private readonly IEmailSender emailSender;

    public TaskExecutor(
        IAppLogger logger,
        IRabbitMq rabbitMq
,
        IEmailSender emailSender)
    {
        this.logger = logger;
        this.rabbitMq = rabbitMq;
        this.emailSender = emailSender;
    }

    public void Start()
    {
        rabbitMq.Subscribe<EmailModel>(QueueNames.EMAIL_CONFIRMATION, async data =>
        {
            logger.Information($"Start sending email confirmation::: {data.To}");

            await emailSender.SendEmailAsync(data); 

            logger.Information($"The confirmation was sent::: {data.To}");
        });
    }
}