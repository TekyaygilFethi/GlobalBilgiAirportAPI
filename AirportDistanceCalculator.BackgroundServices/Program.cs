using AirportDistanceCalculator.BackgroundServices.Managers.Schedulers;
using AirportDistanceCalculator.Business.CacheFolder.Server;
using AirportDistanceCalculator.Business.CacheFolder.Service;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var configuration = builder.Configuration;

#region Hangfire
builder.Services.AddHangfire(options => {
    options.UseSqlServerStorage(configuration.GetSection("ConnectionStrings:Dev")?.Value);
});
builder.Services.AddHangfireServer();

#endregion

#region Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetSection("RedisConfiguration:Url")?.Value;
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<RedisServer>();
#endregion






var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();
InitializeHangfireJobs();

app.MapControllers();

app.Run();




void InitializeHangfireJobs()
{
    RecurringJobsScheduler.CacheAirportJob();
}