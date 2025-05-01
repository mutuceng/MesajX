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
using MesajX.ChatService.Services.ChatRoomServices;
using MesajX.ChatService.Services.MessageServices;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomMemberServices.Redis;
using MesajX.ChatService.Services.ChatRoomMemberServices;
using MassTransit;
using MesajX.ChatService.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.Audience = "ResourceChat";
    opt.RequireHttpsMetadata = false;
});


builder.Services.AddCors(opt => opt.AddPolicy(name: "CorsPolicy", builder =>
    builder.WithOrigins("http://localhost:5173")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()));


builder.Services.AddDbContext<ChatContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    sp.GetRequiredService<IRedisConnectionFactory>().GetConnection()
);

//builder.Services.AddHostedService<MessageSyncService>();

var rabbitMQUri = builder.Configuration["RabbitMQSettings:RabbitMQUri"];
var username = builder.Configuration["RabbitMQSettings:UserName"];
var password = builder.Configuration["RabbitMQSettings:Password"];

if (string.IsNullOrEmpty(rabbitMQUri) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
{
    throw new ArgumentNullException("RabbitMQ konfigürasyonlarý eksik veya null.");
}

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMQUri, configurator =>
        {
            configurator.Username(username);
            configurator.Password(password);
        });

        //cfg.Message<MessageCreatedEvent>(configurator =>
        //{
        //    configurator.SetEntityName("message-created-event");
        //});

        //cfg.ConfigureEndpoints(ctx);

    });
});


builder.Services.AddSignalR();

builder.Services.AddScoped<IRedisMessageService, RedisMessageService>();
builder.Services.AddScoped<IPostgreMessageService, PostgreMessageService>();

builder.Services.AddScoped<IPostgresRoomChatService, PostgresRoomChatService>();
builder.Services.AddScoped<IRedisRoomChatService, RedisRoomChatService>();

builder.Services.AddScoped<IRedisChatMemberService, RedisChatMemberService>();

builder.Services.AddScoped<IChatRoomService, ChatRoomService>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IChatRoomMemberService, ChatRoomMemberService>();


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

app.UseCors("CorsPolicy");
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub");
app.MapControllers();

app.Run();
