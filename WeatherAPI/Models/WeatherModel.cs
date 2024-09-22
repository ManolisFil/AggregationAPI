namespace WeatherAPI.Models
{
    public class WeatherModel
    {
        public string City { get; set; }
        public float Temperature  { get; set; }
        public string Condition  { get; set; }
        public DateTime DataGetDate { get; set; }
    }
}