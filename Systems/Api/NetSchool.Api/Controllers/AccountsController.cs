namespace NetSchool.API.Controllers;

using AutoMapper;
using NetSchool.Services.UserAccount;
using Microsoft.AspNetCore.Mvc;
using NetSchool.Services.UserAccount.Models;
using NetSchool.Services.UserAccount.UserAccount.Models;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("v{version:apiVersion}/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<AccountsController> logger;
    private readonly IUserAccountService userAccountService;

    public AccountsController(IMapper mapper, ILogger<AccountsController> logger, IUserAccountService userAccountService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.userAccountService = userAccountService;
    }

    [HttpPost("")]
    public async Task<UserAccountModel> Register([FromBody] RegisterUserAccountModel request)
    {
        var user = await userAccountService.Create(request);
        return user;
    }

    [HttpGet("")]
    public async Task<UserAccountModel> Get([FromQuery]Guid id)
{
        var user = await userAccountService.Get(id);
        return user;
    }

    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmModel model)
    {
        await userAccountService.ConfirmEmail(model);
        return Ok();
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        await userAccountService.SendEmailToChangePassword(model);
        return Ok();
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        await userAccountService.ChangePassword(model);
        return Ok();
    }

    [HttpPut("Edit")]
    public async Task<IActionResult> EditUserProfile([FromBody] EditProfileModel model)
    {
        await userAccountService.EditUserProfileAsync(model);
        return Ok();
    }
}

