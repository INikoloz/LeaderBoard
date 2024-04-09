using LeaderBoard.Domain.Models;
using LeaderBoard.Domain.ResponseModels;

namespace LeaderBoard.Abstraction.Repositories
{
    public interface IUserRepository
    {
        Task AddUsersAsync(IEnumerable<User> userEntities);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<AllDataResponse>> GetAllDataAsync();
        Task<UserInfoResponse> GetUserInfoAsync(int userId);
    }
}
