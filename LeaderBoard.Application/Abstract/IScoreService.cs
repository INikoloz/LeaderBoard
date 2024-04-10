using LeaderBoard.Application.RequestModels;
using LeaderBoard.Application.ResponseModels;

namespace LeaderBoard.Application.Abstract
{
    public interface IScoreService
    {
        Task<IEnumerable<ScoreResponseModel>> GetScoresByDayAsync(int Day);
        Task<IEnumerable<ScoreResponseModel>> GetScoresByMonthAsync(int month);
        Task UploadUserScoresAsync(UploadUserScoreRequest[] scores);
        Task<StatsResponseModel> GetStatsAsync();
    }
}
