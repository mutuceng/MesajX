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
        Task<bool> IsUserInGroupChatAsync(string chatRoomId, string userId);
    }
}
