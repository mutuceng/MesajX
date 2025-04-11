using AutoMapper;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis;
using MesajX.ChatService.DataAccessLayer.Abstract;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using MesajX.ChatService.EntityLayer.Entities;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.ChatRoomMemberServices.Redis
{
    public class RedisChatMemberService:IRedisChatMemberService
    {
        private readonly IRedisConnectionFactory _redisConnectionFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<RedisRoomChatService> _logger;

        public RedisChatMemberService(IRedisConnectionFactory redisConnectionFactory, IMapper mapper, ILogger<RedisRoomChatService> logger)
        {
            _redisConnectionFactory = redisConnectionFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddMemberToChatAsync(CreateMemberDto createMemberDto)
        {
            try
            {
                var db = _redisConnectionFactory.GetConnection().GetDatabase();
                var key = $"chat:{createMemberDto.ChatRoomId}:members";
                var roomMember = _mapper.Map<ChatRoomMember>(createMemberDto);
                await db.HashSetAsync(key, roomMember.UserId.ToString(), ((int)roomMember.Role).ToString());
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, "Failed to connect to Redis while adding member to chat room {ChatRoomId}", createMemberDto.ChatRoomId);
                throw;
            }


        }

        public async Task RemoveMemberFromChatAsync(string chatRoomId, string userId)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();
            var key = $"chat:{chatRoomId}:members";
            await db.HashDeleteAsync(key, userId);
        }

        public async Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();
            var key = $"chat:{chatRoomId}:members";
            var members = await db.HashGetAllAsync(key);
            var memberDtos = members.Select(m =>
            {
                var memberData = JsonSerializer.Deserialize<Dictionary<string, object>>(m.Value.ToString());

                return new GetMembersByRoomIdDto
                {
                    ChatRoomMemberId = m.Name.ToString(),
                    UserId = memberData["UserId"].ToString(),
                    ChatRoomId = chatRoomId,
                    Role = Enum.Parse<DtoLayer.Dtos.Enums.Role>(memberData["Role"].ToString())
                };
            }).ToList();

            return memberDtos;
        }
    }
}
