using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.Models;
using LeaderBoard.Application.Repositories;
using LeaderBoard.Application.RequestModels;
using LeaderBoard.Application.ResponseModels;

namespace LeaderBoard.Application.Concrete
{
    public class UserScoreService : IScoreService
    {
        private readonly IUserScoreRepository _scoreRepository;

        public UserScoreService(IUserScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public async Task UploadUserScoresAsync(UploadUserScoreRequest[] scores)
        {       
            var scoreEntities = scores.Select(s => new UserScore
            {
                UserId = s.UserId,
                Date = s.Date,
                Score = s.Score
            }).ToArray();

            await _scoreRepository.AddUserScoreAsync(scoreEntities);
      
        }

        public async Task<IEnumerable<ScoreResponseModel>> GetScoresByDayAsync(int day)
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
            var scores = await _scoreRepository.GetUserScoresByDayAsync(date);

            var response = scores.Select(s => new ScoreResponseModel
            {
                UserId = s.UserId,
                UserName = s.UserName,
                TotalScore = s.TotalScore
            });

            return response;
        }

        public async Task<IEnumerable<ScoreResponseModel>> GetScoresByMonthAsync(int month)
        {
            var date = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day);
            return await _scoreRepository.GetUserScoresByMonthAsync(date);
           
        }

        public async Task<StatsResponseModel> GetStatsAsync()
        {
            return await _scoreRepository.GetTopStatsAsync();
           
        }
    }
}
