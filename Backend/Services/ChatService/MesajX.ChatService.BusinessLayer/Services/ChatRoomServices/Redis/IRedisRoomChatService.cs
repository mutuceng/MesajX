using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis
{
    public interface IRedisRoomChatService
    {
        Task AddMemberToChatAsync(CreateMemberDto createMemberDto, string chatRoomId);
        Task RemoveMemberFromChatAsync(string chatRoomId, string userId);
        Task<bool> IsUserInGroupChatAsync(string chatRoomId, string userId);
        Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId);
    }
}
