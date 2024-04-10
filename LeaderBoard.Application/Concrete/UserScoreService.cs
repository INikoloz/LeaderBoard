using LeaderBoard.Abstraction.Repositories;
using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.DTO;
using LeaderBoard.Domain.Models;
using LeaderBoard.Domain.ResponseModels;

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

        public async Task<IEnumerable<ScoreResponse>> GetScoresByDayAsync(int day)
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
            var scores = await _scoreRepository.GetUserScoresByDayAsync(date);

            var response = scores.Select(s => new ScoreResponse
            {
                UserId = s.UserId,
                UserName = s.UserName,
                TotalScore = s.TotalScore
            });

            return response;
        }

        public async Task<IEnumerable<ScoreResponse>> GetScoresByMonthAsync(int month)
        {
            var date = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day);
            return await _scoreRepository.GetUserScoresByMonthAsync(date);
           
        }

        public async Task<StatsResponse> GetStatsAsync()
        {
            return await _scoreRepository.GetTopStatsAsync();
           
        }
    }
}
