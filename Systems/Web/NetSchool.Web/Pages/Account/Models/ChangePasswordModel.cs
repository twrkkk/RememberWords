namespace NetSchool.Web.Pages.Account.Models;

public class ChangePasswordModel
{
    public string Email { get; set; }
    public string Code { get; set; }
    public string NewPassword { get; set; }
}
