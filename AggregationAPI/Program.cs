using AggregationAPI.Service.IService;
using AggregationAPI.Service;
using AggregationAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<ISpotifyService, SpotifyService>();
builder.Services.AddScoped<IBaseService, BaseService>();
 
SD.NewsAPIBase = builder.Configuration["ServiceUrls:NewsAPI"];
SD.WeatherAPIBase = builder.Configuration["ServiceUrls:WeatherAPI"]; 
SD.SpotifyAPIBase = builder.Configuration["ServiceUrls:SpotifyAPI"]; 



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
