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
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis
{
    public class RedisRoomChatService : IRedisRoomChatService
    {
        private readonly IRedisConnectionFactory _redisConnectionFactory;
        private readonly ILogger<RedisRoomChatService> _logger;

        public RedisRoomChatService(IRedisConnectionFactory redisConnectionFactory, ILogger<RedisRoomChatService> logger)
        {
            _redisConnectionFactory = redisConnectionFactory;
            _logger = logger;
        }

        public async Task<bool> IsUserInGroupChatAsync(string chatRoomId, string userId)
        {
            try
            {
                var connection = await _redisConnectionFactory.GetConnectionAsync();

                if (!connection.IsConnected)
                {
                    _logger.LogWarning("Redis is not connected");
                    return false;
                }

                var db = connection.GetDatabase();
                var key = $"chat:{chatRoomId}:members";

                return await db.HashExistsAsync(key, userId);
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError(ex, "Redis connection error while checking user in group");
                return false;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Unexpected error checking user in group chat {ex.Message}");
                return false;
            }
        }

    }
}
