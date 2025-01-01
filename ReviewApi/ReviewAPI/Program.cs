using Microsoft.EntityFrameworkCore;
using ReviewAPI.Configurations;
using ReviewAPI.DbContexts;
using ReviewAPI.Services;
using ReviewAPI.Services.Caching;
using ReviewAPI.Services.Clients;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/reviewApiLogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ReviewContext>(DbContextOptions => DbContextOptions.UseSqlite(
    builder.Configuration["ConnectionStrings:ReviewDBConnectionString"]));

builder.Services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddSingleton<IUserClient, UserClient>();
builder.Services.AddSingleton<IProductClient, ProductClient>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("ApiConfiguration"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IMemoryCacheWrapper, MemoryCacheWrapper>();
builder.Services.AddSingleton<ICache, Cache>();

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
