using LeaderBoard.Domain.Models;
using LeaderBoard.Domain.ResponseModels;

namespace LeaderBoard.Abstraction.Repositories
{
    public interface IUserScoreRepository
    {
        Task<UserScore> AddUserScoreAsync(UserScore[] userScore);
        Task<IEnumerable<ScoreResponse>> GetUserScoresByDayAsync(DateTime day);
        Task<IEnumerable<ScoreResponse>> GetUserScoresByMonthAsync(DateTime month);
        Task<StatsResponse> GetTopStatsAsync();
    }
}
