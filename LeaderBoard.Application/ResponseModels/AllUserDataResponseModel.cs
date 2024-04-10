namespace LeaderBoard.Application.ResponseModels
{
    public class AllUserDataResponseModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? Score { get; set; }
    }
}
