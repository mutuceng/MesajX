using MassTransit;
using MesajX.SyncService;
using MesajX.SyncService.Consumers;
using MesajX.SyncService.SyncServices.MessageSyncService;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageCreatedEventConsumer>();

            // RabbitMQ baðlantý ayarlarý
            x.UsingRabbitMq((ctx, cfg) => // context ve configurator
            {
                cfg.Host(hostContext.Configuration["RABBITMQ_SETTINGS:RabbitMQUrl"], configurator =>
                { 
                    configurator.Username(hostContext.Configuration["RABBITMQ_SETTINGS:Username"]);
                    configurator.Password(hostContext.Configuration["RABBITMQ_SETTINGS:Password"]);
                }); // veya senin hostun

                cfg.ReceiveEndpoint("message-created-event", e =>
                {
                    e.ConfigureConsumer<MessageCreatedEventConsumer>(ctx);
                });
            });
        });

        services.AddSingleton<IMessageSyncService, MessageSyncService>();
        services.AddHostedService<Worker>();

    }).Build();

host.Run();



//var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<MessageCreatedEventConsumer>();

//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("rabbitmq://localhost"); // veya senin hostun

//        cfg.ReceiveEndpoint("message-created-event", e =>
//        {
//            e.ConfigureConsumer<MessageCreatedEventConsumer>(context);
//        });
//    });
//});
