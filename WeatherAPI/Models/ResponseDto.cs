namespace WeatherAPI.Models
{
    public class ResponseDto
    {
        public string Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
