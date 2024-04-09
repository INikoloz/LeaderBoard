namespace LeaderBoard.Domain.ResponseModels
{
    public class ScoreResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalScore { get; set; }
    }
}
