namespace AggregationAPI.Models
{
    public class StatisticsModel
    {
        public int ApiName { get; set; }
        public int TotalRequests { get; set; }
        public int Fast { get; set; }
        public int Average { get; set; }
        public int Slow { get; set; }
    }
}
