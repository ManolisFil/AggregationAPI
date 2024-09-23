namespace AggregationAPI.Models
{
    public class SortModel
    {
        public string City { get; set; }
        public DateTime? Date { get; set; } = new DateTime(2024,1,1);
        public string SortBy { get; set; } = "Date";
        public bool Ascending { get; set; } = true;
    }
}