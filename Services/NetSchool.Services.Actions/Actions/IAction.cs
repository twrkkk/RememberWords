﻿using NetSchool.Services.EmailSender.Models;

namespace NetSchool.Services.Actions;

public interface IAction
{
    Task SendEmailConfirmation(EmailModel model);
}