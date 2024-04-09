﻿using Dapper;
using LeaderBoard.Abstraction.Repositories;
using LeaderBoard.Domain.Models;
using LeaderBoard.Domain.ResponseModels;

namespace LeaderBoard.DAL.Repositories
{
    public class ScoreRepository : IUserScoreRepository
    {
        private readonly DBContext _dbConnection;

        public ScoreRepository(DBContext dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public Task<UserScore> AddUserScoreAsync(UserScore[] userScore)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ScoreResponse>> GetUserScoresByDayAsync(DateTime date)
        {
            using (var connection = await _dbConnection.CreateConnectionAsync())
            {
                try
                {
                    var query = @"
                            SELECT
                                u.Id AS UserId,
                                u.Username,
                                SUM(s.Score) AS TotalScore
                            FROM Scores s
                            JOIN Users u ON s.UserId = u.Id
                            WHERE s.Date = @Date
                            GROUP BY u.Id, u.Username
                            ORDER BY TotalScore DESC";

                    var scores = await connection.QueryAsync<ScoreResponse>(query, new { Date = date });
                    return scores;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                finally
                {
                    await _dbConnection.CloseConnectionAsync(connection);
                }
           
            }
        }

        public async Task<IEnumerable<ScoreResponse>> GetUserScoresByMonthAsync(DateTime month)
        {
            using (var connection = await _dbConnection.CreateConnectionAsync())
            {

                var query = @"
                            SELECT
                                u.Id AS UserId,
                                u.Username,
                                SUM(s.Score) AS TotalScore
                            FROM Scores s
                            JOIN Users u ON s.UserId = u.Id
                            WHERE s.Date BETWEEN @StartDate AND @EndDate
                            GROUP BY u.Id, u.Username
                            ORDER BY TotalScore DESC";

                try
                {
                    var startDate = new DateTime(month.Year, month.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    var scores = await connection.QueryAsync<ScoreResponse>(query, new { StartDate = startDate, EndDate = endDate });
                    return scores;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message); 
                }   
            }
        }

        public async Task<StatsResponse> GetTopStatsAsync()
        {
            using (var connection = await _dbConnection.CreateConnectionAsync())
            {
                var query = @"
                            SELECT 
                                AVG(CAST(TotalScore AS FLOAT)) AS AverageDaily,
                                AVG(CAST(TotalMonthlyScore AS FLOAT)) AS AverageMonthly,
                                MAX(TotalScore) AS MaxDaily,
                                MAX(TotalWeeklyScore) AS MaxWeekly,
                                MAX(TotalMonthlyScore) AS MaxMonthly
                            FROM (
                                SELECT
                                    s.Date,
                                    SUM(s.Score) AS TotalScore,
                                    SUM(SUM(s.Score)) OVER (PARTITION BY DATEPART(WEEK, s.Date), u.Id) AS TotalWeeklyScore,
                                    SUM(SUM(s.Score)) OVER (PARTITION BY DATEPART(MONTH, s.Date), u.Id) AS TotalMonthlyScore
                                FROM Scores s
                                JOIN Users u ON s.UserId = u.Id
                                GROUP BY s.Date, u.Id
                            ) t";

                try
                {
                    var stats = await connection.QuerySingleAsync<StatsResponse>(query);
                    return stats;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }             
            }
        }
    }
     
}