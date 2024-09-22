using Polly;
using Polly.Contrib.WaitAndRetry;
using SpotifyAPI.Service;
using SpotifyAPI.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ISpotifyAccountService, SpotifyAccountService>();
builder.Services.AddScoped<ISpotifyService, SpotifyService>();


builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("SpotifyToken", x => x.BaseAddress = new Uri(builder.Configuration["Spotify:TokenUri"]))
        .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5))); 
builder.Services.AddHttpClient("Spotify",
    x => 
    {
        x.BaseAddress = new Uri(builder.Configuration["Spotify:NewReleasesUri"]);
        x.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    ).AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
