using MesajX.ChatService.BusinessLayer.Abstract;
using MesajX.ChatService.BusinessLayer.Concrete;
using MesajX.ChatService.BusinessLayer.Mapping;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Postgre;
using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Redis;
using MesajX.ChatService.DataAccessLayer.Abstract;
using MesajX.ChatService.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Postgre;
using MesajX.ChatService.BusinessLayer.Services.BackgroundServices;
using MesajX.ChatService.Services.ChatRoomServices;
using MesajX.ChatService.Services.MessageServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ChatContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    sp.GetRequiredService<IRedisConnectionFactory>().GetConnection()
);

builder.Services.AddHostedService<MessageSyncService>();

builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

builder.Services.AddScoped<IRedisMessageService, RedisMessageService>();
builder.Services.AddScoped<IPostgreMessageService, PostgreMessageService>();

builder.Services.AddScoped<IPostgresRoomChatService, PostgresRoomChatService>();
builder.Services.AddScoped<IRedisRoomChatService, RedisRoomChatService>();

builder.Services.AddScoped<IChatRoomService, ChatRoomService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

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
