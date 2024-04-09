using LeaderBoard.Application.DTO;
using LeaderBoard.Domain.ResponseModels;

namespace LeaderBoard.Application.Abstract
{
    public interface IScoreService
    {
        Task<IEnumerable<ScoreResponse>> GetScoresByDayAsync(int Day);
        Task<IEnumerable<ScoreResponse>> GetScoresByMonthAsync(int month);
        Task UploadUserScoresAsync(ScoreDto[] scores);
        Task<StatsResponse> GetStatsAsync();
    }
}
