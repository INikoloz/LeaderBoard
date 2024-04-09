using LeaderBoard.Application.DTO;
using LeaderBoard.Domain.ResponseModels;

namespace LeaderBoard.Application.Abstract
{
    public interface IScoreService
    {
        Task<IEnumerable<ScoreResponse>> GetScoresByDayAsync(DateTime Day);
        Task<IEnumerable<ScoreResponse>> GetScoresByMonthAsync(DateTime month);
        Task UploadUserScoresAsync(ScoreDto[] scores);
        Task<StatsResponse> GetStatsAsync();
    }
}
