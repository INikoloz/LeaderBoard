using LeaderBoard.Abstraction.Repositories;
using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.DTO;
using LeaderBoard.Domain.Models;
using LeaderBoard.Domain.ResponseModels;

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

        public async Task<UserInfoDto> GetUserScoreInfoAsync(int userId)
        {
           var result = await _userRepository.GetUserInfoAsync(userId);
           return new UserInfoDto
           {
               UserId = result.UserId,
               MonthlyScore = result.MonthlyScore,
               Position = result.Position,
               Username = result.Username               
           };
        }

        public async Task<IEnumerable<AllDataResponse>> GetAllUserDataAsync()
        {
            var result = await _userRepository.GetAllDataAsync();
            if (!result.Any()) throw new Exception("");
            return result;
        }
    }
}
