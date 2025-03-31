using MesajX.ChatService.BusinessLayer.Abstract;
using MesajX.ChatService.BusinessLayer.Concrete;
using MesajX.ChatService.BusinessLayer.Mapping;
using MesajX.ChatService.DataAccessLayer.Abstract;
using MesajX.ChatService.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ChatContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    sp.GetRequiredService<IRedisConnectionFactory>().GetConnection()
);

builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(GeneralMapping));
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
