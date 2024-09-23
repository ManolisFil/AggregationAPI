namespace AggregationAPI.Models
{
    public class ResponseModel
    {
        public string Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public int RequestNo { get; set; }
        public long Time { get; set; }
    }
}
