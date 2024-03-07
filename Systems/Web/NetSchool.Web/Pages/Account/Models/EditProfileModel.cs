using System;

namespace NetSchool.Web.Pages.Account.Models;

public class EditProfileModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
