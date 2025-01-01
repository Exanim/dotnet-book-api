using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Configurations;
using OrderApi.DbContexts;
using OrderApi.Services;
using Microsoft.Extensions.Options;
using OrderApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IOrdersContext, OrdersContext>
    (DbContextOptions => DbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:OrdersDBConnectionString"]));
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<IOrdersClients, OrdersClients>();
builder.Services.AddTransient<IOrderMapper, OrderMapper>();
builder.Services.AddSingleton<IMemoryCacheWrapper, MemoryCacheWrapper>();
builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("ApiConfiguration"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMemoryCache();  
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

app.UseMiddleware<ExceptionMiddleware<CustomException>>();

app.MapControllers();

app.Run();
