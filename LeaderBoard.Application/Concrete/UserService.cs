using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.Models;
using LeaderBoard.Application.Repositories;
using LeaderBoard.Application.RequestModels;
using LeaderBoard.Application.ResponseModels;

namespace LeaderBoard.Application.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ValueTask> UploadUserDataAsync(UploadUserDataRequest[] users)
        {
            var userEntities = users.Select(u => new User
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                CreatedAt = DateTime.UtcNow
            });

            await _userRepository.AddUsersAsync(userEntities);
            return ValueTask.CompletedTask; 
        }

        public async Task<UserInfoResponseModel> GetUserScoreInfoAsync(int userId)
        {
           var result = await _userRepository.GetUserInfoAsync(userId);
           return new UserInfoResponseModel
           {
               UserId = result.UserId,
               MonthlyScore = result.MonthlyScore,
               Position = result.Position,
               Username = result.Username               
           };
        }

        public async Task<IEnumerable<AllUserDataResponseModel>> GetAllUserDataAsync()
        {
            var result = await _userRepository.GetAllDataAsync();
            if (!result.Any()) throw new Exception("");
            return result;
        }
    }
}
