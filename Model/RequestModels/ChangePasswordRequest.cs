using System.ComponentModel;

namespace Model.RequestModels;

public class ChangePasswordRequest
{
    public string UserName { get; set; } = String.Empty;
    [DisplayName("Confirm Password")]
    public string CurrentPassword { get; set; } = string.Empty;
    [DisplayName("New Password")]
    public string NewPassword { get; set; } = string.Empty;
}