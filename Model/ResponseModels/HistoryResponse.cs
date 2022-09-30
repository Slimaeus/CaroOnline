namespace Model.ResponseModels;
public class HistoryResponse
{
    public Guid Id { get; set;  }
    public string UserName { get; set; } = string.Empty;
    public string InGameName { get; set; } = string.Empty;

    public string OpponentUserName { get; set; } = string.Empty;
    public string OpponentInGameName { get; set; } = string.Empty;

    public string GameMode { get; set; } = "Default";
    public string Status { get; set; } = string.Empty;
    public int Score { get; set; } = 0;

    public DateTime StartedTime { get; set; } = DateTime.Now;
    public DateTime EndedTime { get; set; } = DateTime.Now;
}
