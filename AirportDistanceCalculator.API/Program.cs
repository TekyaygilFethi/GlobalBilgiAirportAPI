using AirportDistanceCalculator.Business.CacheFolder.Server;
using AirportDistanceCalculator.Business.CacheFolder.Service;
using AirportDistanceCalculator.Business.Middlewares;
using AirportDistanceCalculator.Business.Repositories;
using AirportDistanceCalculator.Business.Services.AirportService;
using AirportDistanceCalculator.Business.Services.User;
using AirportDistanceCalculator.Business.Services.UserService;
using AirportDistanceCalculator.Database.DbContexts;
using GenericDatabaseAccess.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Swagger Implementation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AirportDistanceCalculator.API", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
    c.AddSecurityRequirement(securityRequirement);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Basic"
                                }
                            },
                            new string[] {}
                    }
                });
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});
#endregion
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle




var configuration = builder.Configuration;
ConfigureLogging(configuration);
builder.Host.UseSerilog();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = configuration.GetSection("JwtSettings:Issuer")?.Value,
        ValidateIssuer = true,
        ValidAudience = configuration.GetSection("JwtSettings:Audience")?.Value,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:SigningKey")?.Value)),
        ValidateIssuerSigningKey = true
    };


});

builder.Services.AddDbContext<AirportDistanceCalculatorDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:Dev")?.Value);
});

#region Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetSection("RedisConfiguration:Url")?.Value;
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<RedisServer>();
#endregion


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<AirportDistanceCalculatorDbContext>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();


void ConfigureLogging(IConfiguration configuration)
{
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
        .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration.GetSection("ElasticConfiguration:Url")?.Value))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-").Replace("ı","i")}-{DateTime.Now.Month}-{DateTime.Now.Year}"
    };
}
