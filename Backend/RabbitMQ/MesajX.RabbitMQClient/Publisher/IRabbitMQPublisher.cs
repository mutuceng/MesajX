using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.RabbitMQClient.Publisher
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessage<T>(T message) where T : class;
    }
}
