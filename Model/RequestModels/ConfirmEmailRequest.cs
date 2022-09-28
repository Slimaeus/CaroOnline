namespace Model.RequestModels;

public class ConfirmEmailRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}