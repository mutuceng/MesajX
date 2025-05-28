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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                var connection = await _redisConnectionFactory.GetConnectionAsync();
                var db = connection.GetDatabase();

                var key = $"chat:{createMemberDto.ChatRoomId}:members";

                var roomMember = _mapper.Map<ChatRoomMember>(createMemberDto);
                var memberJson = JsonSerializer.Serialize(roomMember);

                await db.HashSetAsync(key, createMemberDto.UserId, memberJson);

                _logger.LogInformation("Added member {UserId} to chat room {ChatRoomId}",
                    createMemberDto.UserId, createMemberDto.ChatRoomId);
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, "Failed to connect to Redis while adding member to chat room {ChatRoomId}",
                    createMemberDto.ChatRoomId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding member {UserId} to chat room {ChatRoomId}",
                    createMemberDto.UserId, createMemberDto.ChatRoomId);
                throw;
            }
        }

        public async Task RemoveMemberFromChatAsync(string chatRoomId, string userId)
        {
            try
            {
                var connection = await _redisConnectionFactory.GetConnectionAsync();
                var db = connection.GetDatabase();

                var key = $"chat:{chatRoomId}:members";

                var success = await db.HashDeleteAsync(key, userId);

                if (success)
                {
                    _logger.LogInformation("Removed member {UserId} from chat room {ChatRoomId}",
                        userId, chatRoomId);
                }
                else
                {
                    _logger.LogWarning("Member {UserId} not found in chat room {ChatRoomId}",
                        userId, chatRoomId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing member {UserId} from chat room {ChatRoomId}",
                    userId, chatRoomId);
                throw;
            }
        }

        public async Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId)
        {
            try
            {
                var connection = await _redisConnectionFactory.GetConnectionAsync();
                var db = connection.GetDatabase();

                // Hash anahtarı
                var key = $"chat:{chatRoomId}:members";

                // Tüm hash değerlerini getir
                var members = await db.HashGetAllAsync(key);

                if (members == null || members.Length == 0)
                {
                    _logger.LogInformation("No members found in chat room {ChatRoomId}", chatRoomId);
                    return new List<GetMembersByRoomIdDto>();
                }

                var memberDtos = new List<GetMembersByRoomIdDto>();

                foreach (var entry in members)
                {
                    try
                    {
                        // Hash değeri bir JSON string
                        var memberJson = entry.Value.ToString();

                        // JSON'ı nesneye dönüştür
                        var roomMember = JsonSerializer.Deserialize<ChatRoomMember>(memberJson);

                        var memberDto = new GetMembersByRoomIdDto
                        {
                            ChatRoomMemberId = roomMember.ChatRoomMemberId.ToString(),
                            UserId = entry.Name.ToString(), // UserId hash'in anahtarı
                            ChatRoomId = chatRoomId,
                            Role = (DtoLayer.Dtos.Enums.Role)roomMember.Role
                        };

                        memberDtos.Add(memberDto);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogWarning(ex, "Failed to deserialize member {MemberKey} in chat room {ChatRoomId}",
                            entry.Name, chatRoomId);
                    }
                }

                return memberDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting members from chat room {ChatRoomId}", chatRoomId);
                throw;
            }
        }
    }
}
