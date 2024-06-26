﻿using LeaderBoard.Application.Abstract;
using LeaderBoard.Application.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace LeaderBoard.API.Controllers
{
    [Route("api/UserScore")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserScoreController : ControllerBase
    {
        private readonly IScoreService _scoreService;

        public UserScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpPost("UploadUserScores")]
        public async Task<IActionResult> UploadUserScores([FromBody] UploadUserScoreRequest[] scores)
        {
            try
            {
                await _scoreService.UploadUserScoresAsync(scores);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("GetScoresByDay")]
        public async Task<IActionResult> GetScoresByDay([FromQuery]int scoreDate)
        {
            try
            {
                var scores = await _scoreService.GetScoresByDayAsync(scoreDate);
                return Ok(scores);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("GetScoresByMonth")]
        public async Task<IActionResult> GetScoresByMonth([FromQuery] int month)
        {
            try
            {
                var scores = await _scoreService.GetScoresByMonthAsync(month);
                return Ok(scores);
            }
            catch (Exception ex)
            {
        
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var stats = await _scoreService.GetStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                
                return StatusCode(400, ex.Message);
            }
        }
    }
}
