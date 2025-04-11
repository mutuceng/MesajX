using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.RabbitMQClient.Publisher
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<RabbitMQPublisher> _logger;

        public RabbitMQPublisher(IPublishEndpoint publishEndpoint, ILogger<RabbitMQPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task PublishMessage<T>(T message) where T : class 
        {
            _logger.LogInformation("Event Fırlatılıyor: {eventName}", typeof(T).Name);

            try
            {
                await _publishEndpoint.Publish(message);
                _logger.LogInformation("Event Fırlatıldı: {eventName}", typeof(T).Name);
            }
            catch
            {
                _logger.LogError("Event Fırlatılırken hata oluştu: {eventName}", typeof(T).Name);
                throw;
            }
        }
    }
}
