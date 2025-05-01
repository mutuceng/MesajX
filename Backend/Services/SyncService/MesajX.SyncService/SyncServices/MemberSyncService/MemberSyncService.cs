using MassTransit;
using MesajX.ChatService.DataAccessLayer.Concrete;
using MesajX.ChatService.EntityLayer.Entities;
using MesajX.SyncService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.SyncService.SyncServices.MemberSyncService
{
    public class MemberSyncService : IMemberSyncService
    {
        private readonly ChatContext _chatContext;
        private readonly ILogger<MemberSyncService> _logger;

        public MemberSyncService(ChatContext chatContext, ILogger<MemberSyncService> logger)
        {
            _chatContext = chatContext;
            _logger = logger;
        }

        public async Task AddMemberToPostgreAsync(SyncMemberDto syncMemberDto)
        {
            var member = new ChatRoomMember
            {
                UserId = syncMemberDto.UserId,
                ChatRoomId = syncMemberDto.ChatRoomId,
                Role = (ChatService.EntityLayer.Entities.Role)(syncMemberDto.Role ?? Dtos.Role.Member), // Explicitly casting the integer to the Role enum  
            };

            try
            {
                await _chatContext.Set<ChatRoomMember>().AddAsync(member);
                await _chatContext.SaveChangesAsync();
                _logger.LogInformation("Member saved successfully to Postgre: {ChatRoomMemberId}", member.ChatRoomMemberId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving member: {ChatRoomMemberId}", member.ChatRoomMemberId);
                throw;
            }
        }
    }
}
