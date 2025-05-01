using MassTransit;
using MesajX.ChatService.DataAccessLayer.Concrete;
using MesajX.SyncService;
using MesajX.SyncService.Consumers;
using MesajX.SyncService.SyncServices.MemberSyncService;
using MesajX.SyncService.SyncServices.MessageSyncService;
using Microsoft.EntityFrameworkCore;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {

        services.AddDbContext<ChatContext>(options =>
            options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<MessageCreatedEventConsumer>();

            x.AddConsumer<MemberAddedEventConsumer>();


            var rabbitMQUri = hostContext.Configuration["RabbitMQSettings:RabbitMQUri"];
            var username = hostContext.Configuration["RabbitMQSettings:UserName"];
            var password = hostContext.Configuration["RabbitMQSettings:Password"];

            if (string.IsNullOrEmpty(rabbitMQUri) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("RabbitMQ konfigürasyonlarý eksik veya null.");
            }

            // RabbitMQ baðlantý ayarlarý
            x.UsingRabbitMq((ctx, cfg) => // context ve configurator
            {
                cfg.Host(rabbitMQUri, configurator =>
                { 
                    configurator.Username(username);
                    configurator.Password(password);
                });

                cfg.ReceiveEndpoint("message-created-event", e =>
                {
                    e.ConfigureConsumer<MessageCreatedEventConsumer>(ctx);
                });

                cfg.ReceiveEndpoint("member-added-event", e =>
                {
                    e.ConfigureConsumer<MemberAddedEventConsumer>(ctx);
                });

            });
        });

        services.AddScoped<IMessageSyncService, MessageSyncService>();
        services.AddScoped<IMemberSyncService, MemberSyncService>();

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
