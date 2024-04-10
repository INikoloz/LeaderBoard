namespace LeaderBoard.Application.RequestModels
{
    public class UploadUserScoreRequest
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}
