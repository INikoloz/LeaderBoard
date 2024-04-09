using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LeaderBoard.API.Controllers
{
    [Route("api/User")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UploadUserData")]
        public async Task<IActionResult> UploadUserData([FromBody] UserDto[] users)
        {
            try
            {
                await _userService.UploadUserDataAsync(users);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("GetUserInfo/{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            try
            {
                var user = await _userService.GetUserScoreInfoAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpGet("GetAllData")]
        public async Task<IActionResult> GetAllData()
        {
            try
            {
                var scores = await _userService.GetAllUserDataAsync();
                return Ok(scores);
            }
            catch (Exception ex)
            {
 
                return StatusCode(400, ex.Message);
            }
        }
    }
}
