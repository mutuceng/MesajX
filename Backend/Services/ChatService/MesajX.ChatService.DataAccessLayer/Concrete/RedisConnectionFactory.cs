using MesajX.ChatService.DataAccessLayer.Abstract;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DataAccessLayer.Concrete
{
    public class RedisConnectionFactory : IRedisConnectionFactory, IDisposable
    {
        private readonly Lazy<IConnectionMultiplexer> _connectionMultiplexer;

        public RedisConnectionFactory(IConfiguration configuration)
        {
            var redisCongif = ConfigurationOptions.Parse(configuration["Redis:ConnectionString"]);
            redisCongif.AbortOnConnectFail = false;
            redisCongif.ConnectRetry = 5;

            _connectionMultiplexer = new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisCongif));
        }


        public IConnectionMultiplexer GetConnection()
        {
           return _connectionMultiplexer.Value;
        }

        public void Dispose()
        {
            if(_connectionMultiplexer.IsValueCreated && _connectionMultiplexer.Value !=null)
            {
                _connectionMultiplexer.Value.Dispose();
            }
        }

    }
}
