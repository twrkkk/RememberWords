namespace NetSchool.API.Controllers;

using AutoMapper;
using NetSchool.Services.UserAccount;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using NetSchool.Common.Exceptions;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
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
}

