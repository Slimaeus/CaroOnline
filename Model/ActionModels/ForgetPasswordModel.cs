using Model.RequestModels;

namespace Model.ActionModels;

public class ForgetPasswordModel
{
    public GetConfirmCodeRequest Input { get; set; } = default!;
    public string ReturnUrl { get; set; } = string.Empty;
}