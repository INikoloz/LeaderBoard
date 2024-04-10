using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace LeaderBoard.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UploadUserData")]
        public async Task<IActionResult> UploadUserData([FromBody] UploadUserDataRequest [] users)
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
