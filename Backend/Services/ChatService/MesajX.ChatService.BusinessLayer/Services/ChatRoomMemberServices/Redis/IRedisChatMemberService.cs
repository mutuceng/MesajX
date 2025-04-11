using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.ChatRoomMemberServices.Redis
{
    public interface IRedisChatMemberService
    {
        Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId);
        Task AddMemberToChatAsync(CreateMemberDto createMemberDto);
        Task RemoveMemberFromChatAsync(string chatRoomId, string userId);
    }
}
