namespace NetSchool.Services.UserAccount;

using AutoMapper;
using NetSchool.Common.Exceptions;
using NetSchool.Common.Validator;
using NetSchool.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetSchool.Services.Actions;
using System;
using NetSchool.Services.EmailSender.Models;
using NetSchool.Services.UserAccount.Models;
using System.Web;
using MimeKit;
using System.Net.Mail;

public class UserAccountService : IUserAccountService
{
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator;
    private readonly IAction action;

    public UserAccountService(
        IMapper mapper,
        UserManager<User> userManager,
        IModelValidator<RegisterUserAccountModel> registerUserAccountModelValidator,
        IAction action
        )
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.registerUserAccountModelValidator = registerUserAccountModelValidator;
        this.action = action;
    }

    public async Task<bool> IsEmpty()
    {
        return !(await userManager.Users.AnyAsync());
    }

    public async Task<UserAccountModel> Create(RegisterUserAccountModel model)
    {
        registerUserAccountModelValidator.Check(model);

        // Find user by email
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
            throw new ProcessException($"User account with email {model.Email} already exist.");

        // Create user account
        user = new User()
        {
            Status = UserStatus.Active,
            UserName = model.UserName,
            Email = model.Email,
            EmailConfirmed = false,
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            RegistrationDate = DateTime.UtcNow,
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            throw new ProcessException($"Creating user account is wrong. {string.Join(", ", result.Errors.Select(s => s.Description))}");

        await SendEmailConfirmation(user);

        return mapper.Map<UserAccountModel>(user);
    }

    public async Task SendEmailConfirmation(User user)
    {
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var uriBuilder = new UriBuilder("https", "localhost", 7165, "/ConfirmEmail");
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["userEmail"] = user.Email;
        query["code"] = code;
        uriBuilder.Query = query.ToString();

        var callbackUrl = uriBuilder.ToString();

        var email = new EmailModel
        {
            To = user.Email,
            Subject = "Memorizing Email Confirmation",
            Content = string.Format("Memorizing team thanks you for registation!\nTo confirm email, click <a href='{0}'>here</a>", callbackUrl)
        };

        await action.SendEmailConfirmation(email);
    }

    public async Task ConfirmEmail(EmailConfirmModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
            throw new EntityNotFoundException($"User (EMAIL = {model.Email}) not found.");

        await userManager.ConfirmEmailAsync(user, model.Code);
    }

    public async Task SendEmailToChangePassword(ResetPasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
            throw new EntityNotFoundException($"User (EMAIL = {model.Email}) not found.");

        var code = await userManager.GeneratePasswordResetTokenAsync(user);

        var uriBuilder = new UriBuilder("https", "localhost", 7165, "/change-password");
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["userEmail"] = user.Email;
        query["code"] = code;
        uriBuilder.Query = query.ToString();

        var callbackUrl = uriBuilder.ToString();

        var email = new EmailModel
        {
            To = user.Email,
            Subject = "Memorizing Change Password",
            Content = string.Format("To reset password on Memorizing, click <a href='{0}'>here</a>", callbackUrl)
        };

        await action.SendResetPasswordEmail(email);
    }

    public async Task ChangePassword(ChangePasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
            throw new EntityNotFoundException($"User (EMAIL = {model.Email}) not found.");

        await userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
    }
}
