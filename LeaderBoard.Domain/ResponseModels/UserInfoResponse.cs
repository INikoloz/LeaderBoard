namespace LeaderBoard.Domain.ResponseModels
{
    public class UserInfoResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Position { get; set; }
        public int MonthlyScore { get; set; }
    }
}
