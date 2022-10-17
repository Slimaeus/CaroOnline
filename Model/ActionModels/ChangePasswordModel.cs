using Model.RequestModels;
using System.ComponentModel;

namespace Model.ActionModels;

public class ChangePasswordModel
{
    public ChangePasswordRequest Input { get; set; } = new ChangePasswordRequest();
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
