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

        public async Task UploadUserScoresAsync(ScoreDto[] scores)
        {       
            var scoreEntities = scores.Select(s => new UserScore
            {
                UserId = s.UserId,
                Date = s.Date,
                Score = s.Score
            }).ToArray();

           var ScoreEntity =  await _scoreRepository.AddUserScoreAsync(scoreEntities);
      
        }   

        public async Task<IEnumerable<ScoreResponse>> GetScoresByDayAsync(DateTime day)
        {
            return await _scoreRepository.GetUserScoresByDayAsync(day);

      
        }

        public async Task<IEnumerable<ScoreResponse>> GetScoresByMonthAsync(DateTime month)
        {
            return await _scoreRepository.GetUserScoresByMonthAsync(month);
           
        }

        public async Task<StatsResponse> GetStatsAsync()
        {
            return await _scoreRepository.GetTopStatsAsync();
           
        }
    }
}
