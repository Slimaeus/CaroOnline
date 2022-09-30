namespace Model.ResponseModels;
public class ResultResponse
{
    public Guid Id { get; }
    public DateTime StartedTime { get; set; } = DateTime.Now;
    public DateTime EndedTime { get; set; } = DateTime.Now;

    public int LimitTime { get; set; }
}

