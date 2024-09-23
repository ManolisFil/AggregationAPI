namespace AggregationAPI.Models
{
    public class StatisticsModel
    {
        public string ApiName { get; set; }
        public int TotalRequests { get; set; }
        public long Time { get; set; }
        public long AverageResponseTime { get; set; }
        public string Performance { get; set; } = "None";
    }
}
