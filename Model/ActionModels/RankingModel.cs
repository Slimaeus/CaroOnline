using Model.ResponseModels;

namespace Model.ActionModels;

public class RankingModel
{
    public PagedList<RankingResponse> AllPlayerRanks { get; set; } = new PagedList<RankingResponse>();
    public RankingResponse PlayerRank { get; set; } = new RankingResponse();
}
