namespace AggregationAPI.Models
{
    public class RequestModel
    {
        public string Url { get; set; }
        public object Data { get; set; } // in case we need a post request in the future
    }
}
