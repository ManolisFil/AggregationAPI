namespace NewsAPI.Models
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}

public class Rootobject
{
    public string status { get; set; }
    public int totalResults { get; set; }
    public Article[] articles { get; set; }
}

public class Article
{
    public Source source { get; set; }
    public string author { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string url { get; set; }
    public string urlToImage { get; set; }
    public DateTime publishedAt { get; set; }
    public string content { get; set; }
}

public class Source
{
    public object id { get; set; }
    public string name { get; set; }
}
