namespace LeaderBoard.Application.ResponseModels
{
    public class StatsResponseModel
    {
        public double AverageDaily { get; set; }
        public double AverageMonthly { get; set; }
        public int MaxDaily { get; set; }
        public int MaxWeekly { get; set; }
        public int MaxMonthly { get; set; }
    }
}
