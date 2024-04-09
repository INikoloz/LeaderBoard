using Dapper;
using LeaderBoard.Abstraction.Repositories;
using LeaderBoard.Domain.Models;
using LeaderBoard.Domain.ResponseModels;
using System.Data;
using System.Data.SqlClient;

namespace LeaderBoard.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;
        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUsersAsync(IEnumerable<User> users)
        {
            var query = "INSERT INTO Users (FirstName, LastName, Username, CreatedAt) VALUES (@FirstName, @LastName, @Username, @CreatedAt)";

            using (var connection = await _dbContext.CreateConnectionAsync())
            {
                try
                {
                    var affectedRows = await connection.ExecuteAsync(query, users);
                }
                catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
                {
                    throw new Exception($"Duplicate usernames found: {string.Join(", ", users.Where(u => ex.Message.Contains(u.Username)).Select(u => u.Username))}", ex);
                }
                finally
                {
                    await _dbContext.CloseConnectionAsync(connection);
                }
            }
        }
        public async Task<IEnumerable<AllDataResponse>> GetAllDataAsync()
        {
            using (var connection = await _dbContext.CreateConnectionAsync())
            {
                try
                {
                    var query = @"
                            SELECT
                                u.Id AS UserId,
                                u.Username,
                                s.Score
                            FROM Users u
                            LEFT JOIN Scores s ON u.Id = s.UserId
                            ORDER BY u.Id, s.Date DESC";

                    var data = await connection.QueryAsync<AllDataResponse>(query);
                    return data;
                }
                catch (Exception ex )
                {

                    throw new Exception(ex.Message);
                }
                finally
                {
                    await _dbContext.CloseConnectionAsync(connection);
                }
              
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";

            using (var connection = await _dbContext.CreateConnectionAsync())
            {
                try
                {
                    var result = await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = userId });
                    if (result is not null)
                    {
                        return result;
                    }
                    throw new Exception("user not found");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    await _dbContext.CloseConnectionAsync(connection);
                }
            }
        }

        public async Task<UserInfoResponse> GetUserInfoAsync(int userId)
        {
            using (var connection = await _dbContext.CreateConnectionAsync())
            {
                var query = @"
                            WITH UserScores AS (
                                SELECT
                                    s.UserId,
                                    SUM(s.Score) AS MonthlyScore,
                                    DENSE_RANK() OVER (ORDER BY SUM(s.Score) DESC) AS Position
                                FROM Scores s
                                WHERE s.Date BETWEEN @StartDate AND @EndDate
                                GROUP BY s.UserId
                            )
                            SELECT
                                u.Id AS UserId,
                                u.Username,
                                us.Position,
                                us.MonthlyScore
                            FROM Users u
                            LEFT JOIN UserScores us ON u.Id = us.UserId
                            WHERE u.Id = @UserId";

                try
                {
                    var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    var userInfo = await connection.QuerySingleOrDefaultAsync<UserInfoResponse>(query, new { UserId = userId, StartDate = startDate, EndDate = endDate });
                    return userInfo ?? new UserInfoResponse { UserId = userId };
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                finally
                {
                    await _dbContext.CloseConnectionAsync(connection);
                }

               
            }
        }
    }
}
