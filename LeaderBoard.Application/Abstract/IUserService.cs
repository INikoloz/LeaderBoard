using LeaderBoard.Application.RequestModels;
using LeaderBoard.Application.ResponseModels;

namespace LeaderBoard.Application.Abstract
{
    public interface IUserService
    {
        Task<UserInfoResponseModel> GetUserScoreInfoAsync(int userId);
        Task<ValueTask> UploadUserDataAsync(UploadUserDataRequest[] users);
        Task<IEnumerable<AllUserDataResponseModel>> GetAllUserDataAsync();
    }
}
