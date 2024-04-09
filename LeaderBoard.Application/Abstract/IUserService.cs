﻿using LeaderBoard.Application.DTO;
using LeaderBoard.Domain.ResponseModels;

namespace LeaderBoard.Application.Abstract
{
    public interface IUserService
    {
        Task<UserInfoDto> GetUserScoreInfoAsync(int userId);
        Task<ValueTask> UploadUserDataAsync(UserDto[] users);
        Task<IEnumerable<AllDataResponse>> GetAllUserDataAsync();
    }
}