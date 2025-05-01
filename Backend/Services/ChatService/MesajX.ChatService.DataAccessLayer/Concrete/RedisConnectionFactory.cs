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
            var redisConfig = ConfigurationOptions.Parse(configuration["Redis:ConnectionString"]);
            redisConfig.AbortOnConnectFail = false;
            redisConfig.ConnectRetry = 5;
            redisConfig.AllowAdmin = true;

            _connectionMultiplexer = new Lazy<IConnectionMultiplexer>(() =>
            {
                var connection = ConnectionMultiplexer.Connect(redisConfig);

                var server = connection.GetServer("localhost", 6380); // Sunucu adresi ve portu
                server.ConfigSet("maxmemory", "100mb"); // Memory limit
                server.ConfigSet("maxmemory-policy", "allkeys-lru"); // LRU politikası
                return connection;
            });
            //_connectionMultiplexer = new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisCongif));
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
