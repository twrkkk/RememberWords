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
using NetSchool.Services.UserAccount.UserAccount.Models;

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

    public async Task<bool> IsEmptyAsync()
    {
        return !(await userManager.Users.AnyAsync());
    }

    public async Task<UserAccountModel> CreateAsync(RegisterUserAccountModel model)
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

        await SendEmailConfirmationAsync(user);

        return mapper.Map<UserAccountModel>(user);
    }

    public async Task SendEmailConfirmationAsync(UserIdModel model)
    {
        if (model == null || model.userId == Guid.Empty)
            return;

        var user = await userManager.FindByIdAsync(model.userId.ToString());

        if (user == null)
            throw new EntityNotFoundException($"User (Id = {model.userId}) not found.");

        await SendEmailConfirmationAsync(user);
    }

    public async Task SendEmailConfirmationAsync(User user)
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

        await action.SendEmailConfirmationAsync(email);
    }

    public async Task ConfirmEmailAsync(EmailConfirmModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
            throw new EntityNotFoundException($"User (EMAIL = {model.Email}) not found.");

        await userManager.ConfirmEmailAsync(user, model.Code);
    }

    public async Task SendEmailToChangePasswordAsync(ResetPasswordModel model)
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

        await action.SendResetPasswordEmailAsync(email);
    }

    public async Task ChangePasswordAsync(ChangePasswordModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
            throw new EntityNotFoundException($"User (EMAIL = {model.Email}) not found.");

        await userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
    }

    public async Task<UserAccountModel> GetAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
            throw new EntityNotFoundException($"User (Id = {id}) not found.");

        return mapper.Map<UserAccountModel>(user);
    }

    public async Task EditUserProfileAsync(EditProfileModel model)
    {
        var user = await userManager.FindByIdAsync(model.Id.ToString());

        if (user == null)
            throw new EntityNotFoundException($"User (Id = {model.Id}) not found.");

        if (user.Email != model.Email)
            user.EmailConfirmed = false;
        user.UserName = model.UserName;
        user.Email = model.Email;

        await userManager.UpdateAsync(user);

        await SendEmailConfirmationAsync(user);
    }

    public async Task SubscribeAsync(SubscribeModel model)
    {
        await SubscriptionHandling(model, true);
    }

    public async Task UnsubscribeAsync(SubscribeModel model)
    {
        await SubscriptionHandling(model, false);
    }

    private async Task SubscriptionHandling(SubscribeModel model, bool subscribe)
    {
        var user = await userManager.FindByIdAsync(model.UserId.ToString());
        if (user == null)
            throw new EntityNotFoundException($"User (Id = {model.UserId}) not found.");

        var following = await userManager.FindByIdAsync(model.FollowId.ToString());
        if (following == null)
            throw new EntityNotFoundException($"User (Id = {model.FollowId}) not found.");

        if (subscribe)
        {
            user.Following.Add(following);
            following.Followers.Add(user);

        }
        else
        {
            user.Following.Remove(following);
            following.Followers.Remove(user);
        }

        await userManager.UpdateAsync(user);
    }
}
