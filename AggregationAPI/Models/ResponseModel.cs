namespace AggregationAPI.Models
{
    public class ResponseModel
    {
        public string Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public double Time { get; set; }
    }
}
