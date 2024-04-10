namespace LeaderBoard.Application.DTO
{
    public class UploadUserScoreRequest
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}
