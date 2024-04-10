using LeaderBoard.Application.Models;
using LeaderBoard.Application.ResponseModels;

namespace LeaderBoard.Application.Repositories
{
    public interface IUserRepository
    {
        Task AddUsersAsync(IEnumerable<User> userEntities);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<AllUserDataResponseModel>> GetAllDataAsync();
        Task<UserInfoResponseModel> GetUserInfoAsync(int userId);
    }
}
