using MassTransit;
using Microsoft.AspNetCore.Cors.Infrastructure;
using SignalRRealTimeAPI.Consumers;
using SignalRRealTimeAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(opt => opt.AddPolicy(name: "CorsPolicy", builder => 
    builder.WithOrigins("http://localhost:5173")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()));

builder.Services.AddSignalR();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MessageCreatedEventConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RABBITMQ_SETTINGS:RabbitMQUrl"], configurator =>
        {
            configurator.Username(builder.Configuration["RABBITMQ_SETTINGS:Username"]);
            configurator.Password(builder.Configuration["RABBITMQ_SETTINGS:Password"]);
        });

        cfg.ReceiveEndpoint("message-created-event", e =>
        {
            e.ConfigureConsumer<MessageCreatedEventConsumer>(ctx);
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");
app.MapHub<NotificationHub>("/notificationhub");

app.Run();
