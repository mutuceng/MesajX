using AutoMapper;
using MesajX.ChatService.DataAccessLayer.Abstract;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using MesajX.ChatService.EntityLayer.Entities;
using MesajX.ChatService.DtoLayer.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Role = MesajX.ChatService.DtoLayer.Dtos.Enums.Role;

namespace MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis
{
    public class RedisRoomChatService : IRedisRoomChatService
    {
        private readonly IRedisConnectionFactory _redisConnectionFactory;
        private readonly IMapper _mapper;

        public RedisRoomChatService(IRedisConnectionFactory redisConnectionFactory, IMapper mapper)
        {
            _redisConnectionFactory = redisConnectionFactory;
            _mapper = mapper;
        }

        public async Task AddMemberToChatAsync(CreateMemberDto createMemberDto, string chatRoomId)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();
            var key = $"chat:{chatRoomId}:members";
            var roomMember = _mapper.Map<ChatRoomMember>(createMemberDto);
            await db.HashSetAsync(key, roomMember.UserId.ToString(), ((int)roomMember.Role).ToString());
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
                    Role = Enum.Parse<Role>(memberData["Role"].ToString())
                };
            }).ToList();

            return memberDtos;
        }

        public async Task<bool> IsUserInGroupChatAsync(string chatRoomId, string userId)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();
            var key = $"chat:{chatRoomId}:members";
            return await db.HashExistsAsync(key, userId);
        }

        public async Task RemoveMemberFromChatAsync(string chatRoomId, string userId)
        {
            var db = _redisConnectionFactory.GetConnection().GetDatabase();
            var key = $"chat:{chatRoomId}:members";
            await db.HashDeleteAsync(key, userId);
        }
    }
}
