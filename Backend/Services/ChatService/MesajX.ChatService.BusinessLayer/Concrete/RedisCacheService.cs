using AutoMapper;
using MesajX.ChatService.BusinessLayer.Abstract;
using MesajX.ChatService.DataAccessLayer.Abstract;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.EntityLayer.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Concrete
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IRedisConnectionFactory _redisConnectionFactory;
        private readonly IMapper _mapper;

        public RedisCacheService(IRedisConnectionFactory redisConnectionFactory, IMapper mapper)
        {
            _redisConnectionFactory = redisConnectionFactory;
            _mapper = mapper;
        }

        private IDatabase GetDatabase()
        {
            return _redisConnectionFactory.GetConnection().GetDatabase();
        }

        public async Task<List<GetMessagesDto>> GetMessagesAsync(string groupId, int count)
        {
            var db = GetDatabase();
            var key = $"chat:{groupId}:message";
            var messages = await db.ListRangeAsync(key,0,count-1);

            if (messages.Length == 0)
                return new List<GetMessagesDto>();

            var result = new List<GetMessagesDto>();

            foreach (var x in messages)
            {
                try
                {
                    var message = JsonSerializer.Deserialize<Message>(x);
                    if (message != null)
                    {
                        result.Add(_mapper.Map<GetMessagesDto>(message));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Mesaj deserialize edilirken hata: {ex.Message}");
                }
            }
            return result;
        }

        public async Task<bool> IsUserInGroupChatAsync(string groupId, string userId)
        {
            var db = GetDatabase();
            var key = $"chat:{groupId}:members";
            return await db.SetContainsAsync(key, userId.ToString());
        }

        public async Task SetMessageAsync(SendMessageDto sendMessageDto)
        {
            var db = GetDatabase();
            var key = $"chat:{sendMessageDto.ChatRoomId}:messages";
            await db.ListLeftPushAsync(key, JsonSerializer.Serialize(sendMessageDto));
            await db.KeyExpireAsync(key, TimeSpan.FromHours(24));
        }

        public Task AddMemberToRoomAsync(string chatRoomId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
