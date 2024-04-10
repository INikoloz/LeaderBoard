namespace LeaderBoard.Application.ResponseModels
{
    public class UserInfoResponseModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Position { get; set; }
        public int MonthlyScore { get; set; }
    }
}
