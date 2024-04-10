using Dapper;
using LeaderBoard.Application.Models;
using LeaderBoard.Application.Repositories;
using LeaderBoard.Application.ResponseModels;

namespace LeaderBoard.DAL.Repositories
{
    public class ScoreRepository : IUserScoreRepository
    {
        private readonly DBContext _dbConnection;

        public ScoreRepository(DBContext dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task AddUserScoreAsync(UserScore[] userScores)
        {
            var query = "INSERT INTO UserScores (UserId, Score, Date) VALUES (@UserId, @Score, @Date)";

            using (var connection = await _dbConnection.CreateConnectionAsync())
            {
                try
                {
                    await connection.ExecuteAsync(query, userScores);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await _dbConnection.CloseConnectionAsync(connection);
                }
            }
        }

        public async Task<IEnumerable<ScoreResponseModel>> GetUserScoresByDayAsync(DateTime date)
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
                            FROM UserScores s
                            JOIN Users u ON s.UserId = u.Id
                            WHERE s.Date BETWEEN @StartDate AND @EndDate
                            GROUP BY u.Id, u.Username
                            ORDER BY TotalScore DESC"
                    ;

                    var startDate = date;
                    var endDate = date.AddDays(1).Date;

                    var scores = await connection.QueryAsync<ScoreResponseModel>(query, new { StartDate = startDate, EndDate = endDate });
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

        public async Task<IEnumerable<ScoreResponseModel>> GetUserScoresByMonthAsync(DateTime month)
        {
            using (var connection = await _dbConnection.CreateConnectionAsync())
            {

                var query = @"
                            SELECT
                                u.Id AS UserId,
                                u.Username,
                                SUM(s.Score) AS TotalScore
                            FROM UserScores s
                            JOIN Users u ON s.UserId = u.Id
                            WHERE s.Date BETWEEN @StartDate AND @EndDate
                            GROUP BY u.Id, u.Username
                            ORDER BY TotalScore DESC";

                try
                {
                    var startDate = new DateTime(month.Year, month.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    var scores = await connection.QueryAsync<ScoreResponseModel>(query, new { StartDate = startDate, EndDate = endDate });
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

        public async Task<StatsResponseModel> GetTopStatsAsync()
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
                                FROM UserScores s
                                JOIN Users u ON s.UserId = u.Id
                                GROUP BY s.Date, u.Id
                            ) t";

                try
                {
                    var stats = await connection.QuerySingleAsync<StatsResponseModel>(query);
                    return stats;
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
    }
     
}
