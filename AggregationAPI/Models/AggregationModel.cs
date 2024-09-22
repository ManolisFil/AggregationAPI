namespace AggregationAPI.Models
{
    public class AggregationModel
    {
        public WeatherModel Weather { get; set; }
        public List<NewsModel> News { get; set; }
        public List<ReleaseModel> Release { get; set; }
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = true;
    }
}
