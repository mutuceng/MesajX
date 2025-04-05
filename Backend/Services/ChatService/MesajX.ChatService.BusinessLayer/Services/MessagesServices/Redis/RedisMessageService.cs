using AutoMapper;
using MesajX.ChatService.DataAccessLayer.Abstract;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.EntityLayer.Entities;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.MessagesServices.Redis
{
    public class RedisMessageService : IRedisMessageService
    {
        private readonly IRedisConnectionFactory _redisConnectionFactory;
        private readonly IMapper _mapper;
        private readonly TimeSpan _messageTtl = TimeSpan.FromDays(7);

        public RedisMessageService(IRedisConnectionFactory connectionFactory, IMapper mapper)
        {
            _redisConnectionFactory = connectionFactory;
            _mapper = mapper;
        }

        public async Task<List<GetMessagesDto>> GetRecentMessagesAsync(GetRecentMessagesRequestDto getRecentMessagesDto, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested(); // İşlemin başında iptal kontrolü

            var db = _redisConnectionFactory.GetConnection().GetDatabase();

            var messagesJson = await db.ListRangeAsync($"chat:{getRecentMessagesDto.ChatRoomId}:messages", 0, getRecentMessagesDto.Count - 1);

            cancellationToken.ThrowIfCancellationRequested(); // Redis’ten veri çekildikten sonra kontrol

            //var messages = messagesJson
            //  .Select(msg => JsonSerializer.Deserialize<GetMessagesDto>(msg))
            //  .Where(msg => msg != null)
            //  .Select(msg => _mapper.Map<GetMessagesDto>(msg))
            //  .ToList(); CancelationToken ile LINQ sorguları calıstırılamıyor.

            var messages = new List<GetMessagesDto>();
            foreach (var msg in messagesJson)
            {
                cancellationToken.ThrowIfCancellationRequested(); // Her bir öğe işlenmeden önce kontrol

                var message = JsonSerializer.Deserialize<GetMessagesDto>(msg);
                if (getRecentMessagesDto.LastMessageDate == null || message.SentAt > getRecentMessagesDto.LastMessageDate)
                {
                    messages.Add(message);
                }
            }

            return messages; 
        }

        public async Task SetMessageAsync(SendMessageDto sendMessageDto)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();

            var message = _mapper.Map<SendMessageDto>(sendMessageDto);
            

            var messageJson = JsonSerializer.Serialize(message);
            var redisKey = $"chat:{sendMessageDto.ChatRoomId}:messages";

            await db.ListRightPushAsync(redisKey, messageJson);

            var ttl = await db.KeyTimeToLiveAsync(redisKey);
            if (ttl == null)
            {
                await db.KeyExpireAsync(redisKey, _messageTtl);
            }
        }

    }
}
