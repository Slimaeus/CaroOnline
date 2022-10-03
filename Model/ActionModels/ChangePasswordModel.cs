using Model.RequestModels;

namespace Model.ActionModels;

public class ChangePasswordModel
{
    public ChangePasswordRequest Input { get; set; } = new ChangePasswordRequest();
    public string ConfirmPassword { get; set; } = string.Empty;
}
