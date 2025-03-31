using MesajX.ChatService.BusinessLayer.Abstract;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Concrete
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public Task<List<GetMessagesDto>> GetMessagesAsync(int count)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInGroupChatAsync(string groupId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task SetMessageAsync(SendMessageDto sendMessageDto)
        {
            throw new NotImplementedException();
        }
    }
}
