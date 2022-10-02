namespace Model.ResponseModels;

public class RankingResponse
{
    public int Top { get; set; } = 0;
    public string InGameName { get; set; } = string.Empty;
    public int Level { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Win { get; set; }
    public int Draw { get; set; }
    public int Lose { get; set; }
    public int Score { get; set; }
}
