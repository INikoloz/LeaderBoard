using Dapper;
using LeaderBoard.Application.Models;
using LeaderBoard.Application.Repositories;
using LeaderBoard.Application.ResponseModels;
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
        public async Task<IEnumerable<AllUserDataResponseModel>> GetAllDataAsync()
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
                            LEFT JOIN UserScores s ON u.Id = s.UserId
                            ORDER BY u.Id, s.Date DESC";

                    var data = await connection.QueryAsync<AllUserDataResponseModel>(query);
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

        public async Task<UserInfoResponseModel> GetUserInfoAsync(int userId)
        {
            using (var connection = await _dbContext.CreateConnectionAsync())
            {
             

                var query = @"
                            SELECT
                                u.Id AS UserId,
                                u.Username,
                                (
                                    SELECT
                                        DENSE_RANK() OVER (ORDER BY SUM(s.Score) DESC)
                                    FROM UserScores s
                                    WHERE s.UserId = u.Id
                                        AND s.Date BETWEEN @StartDate AND @EndDate
                                ) AS Position,
                                (
                                    SELECT
                                        SUM(s.Score)
                                    FROM UserScores s
                                    WHERE s.UserId = u.Id
                                        AND s.Date BETWEEN @StartDate AND @EndDate
                                ) AS MonthlyScore
                            FROM Users u
                            WHERE u.Id = @UserId";

                try
                {
                    var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    var userInfo = await connection.QuerySingleOrDefaultAsync<UserInfoResponseModel>(query, new { UserId = userId, StartDate = startDate, EndDate = endDate });
                    return userInfo ?? new UserInfoResponseModel { UserId = userId };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await _dbContext.CloseConnectionAsync(connection);
                }
            }
        }
    }
}
