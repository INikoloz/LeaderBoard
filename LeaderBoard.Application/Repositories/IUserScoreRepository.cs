using LeaderBoard.Application.Models;
using LeaderBoard.Application.ResponseModels;

namespace LeaderBoard.Application.Repositories
{
    public interface IUserScoreRepository
    {
        Task AddUserScoreAsync(UserScore[] userScore);
        Task<IEnumerable<ScoreResponseModel>> GetUserScoresByDayAsync(DateTime day);
        Task<IEnumerable<ScoreResponseModel>> GetUserScoresByMonthAsync(DateTime month);
        Task<StatsResponseModel> GetTopStatsAsync();
    }
}
