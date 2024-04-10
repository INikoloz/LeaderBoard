namespace LeaderBoard.Domain.Models
{
    public class UserScore
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; } 

    }
}
