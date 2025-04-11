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
        private readonly TimeSpan _messageTtl = TimeSpan.FromDays(1);
        public RedisMessageService(IRedisConnectionFactory connectionFactory, IMapper mapper)
        {
            _redisConnectionFactory = connectionFactory;
            _mapper = mapper;
        }

        public async Task<List<GetMessagesDto>> GetRecentMessagesAsync(GetRecentMessagesRequestDto getRecentMessagesDto)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();

            // Redis'ten mesajları almak için doğru aralık hesaplanmalı
            int startIndex = (getRecentMessagesDto.Page - 1) * getRecentMessagesDto.PageSize;
            int endIndex = startIndex + getRecentMessagesDto.PageSize - 1;

            var messagesJson = await db.ListRangeAsync($"chat:{getRecentMessagesDto.ChatRoomId}:messages", startIndex, endIndex);

            var messages = new List<GetMessagesDto>();

            // Her bir mesaj üzerinde iptal kontrolü yaparak işlemi sürdürüyoruz
            foreach (var msg in messagesJson)
            {
                var message = JsonSerializer.Deserialize<GetMessagesDto>(msg);
                if (message != null)
                {
                    messages.Add(message);
                }
            }

            // Sayfalama ile ilgili son kontrolleri ekleyebilirsiniz
            return messages;
        }


        public async Task SetMessageAsync(SendMessageDto sendMessageDto)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();

            var message = _mapper.Map<SendMessageDto>(sendMessageDto);

            // Eğer MediaUrl null ise, değeri null 
            if (string.IsNullOrEmpty(message.MediaUrl))
            {
                message.MediaUrl = null;  
            }

            // Mesajı JSON formatında serileştiriyoruz.
            var messageJson = JsonSerializer.Serialize(message);
            var redisKey = $"chat:{sendMessageDto.ChatRoomId}:messages";

            // Mesajı Redis'e ekliyoruz.
            await db.ListRightPushAsync(redisKey, messageJson);

            // TTL süresi kontrolü.
            var ttl = await db.KeyTimeToLiveAsync(redisKey);
            if (ttl == null)
            {
                await db.KeyExpireAsync(redisKey, _messageTtl);
            }
        }


    }
}
