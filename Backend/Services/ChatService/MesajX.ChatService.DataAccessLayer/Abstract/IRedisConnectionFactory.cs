using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DataAccessLayer.Abstract
{
    public interface IRedisConnectionFactory
    {
        Task<IConnectionMultiplexer> GetConnectionAsync();
    }
}
