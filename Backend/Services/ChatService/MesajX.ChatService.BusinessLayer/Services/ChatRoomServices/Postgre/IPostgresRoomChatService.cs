using MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Postgre
{
    public interface IPostgresRoomChatService
    {
        Task CreateChatRoomAsync(CreateChatRoomDto createChatRoomDto);
        Task UpdateChatRoomAsync(UpdateChatRoomDto updateChatRoomDto);
        Task DeleteChatRoomAsync(string chatId);
        Task<List<GetChatRoomByUserIdDto>> GetChatsByUserId(string userId);
        Task<GetByIdChatRoomDto> GetChatRoomByIdAsync(string chatRoomId);
       
    }
}
