using AutoMapper;
using MesajX.ChatService.DataAccessLayer.Concrete;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using MesajX.ChatService.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Postgre
{
    public class PostgresRoomChatService : IPostgresRoomChatService
    {
        private readonly ChatContext _chatContext;
        private readonly IMapper _mapper;
        public PostgresRoomChatService(IMapper mapper, ChatContext chatContext)
        {
            _mapper = mapper;
            _chatContext = chatContext;
        }

        public async Task CreateChatRoomAsync(CreateChatRoomDto createChatRoomDto)
        {
            var room = _mapper.Map<ChatRoom>(createChatRoomDto);
            await _chatContext.AddAsync(room);
            await _chatContext.SaveChangesAsync();
        }

        //public async Task DeleteAllMessages(string chatId)
        //{
        //    var messages = await _chatContext.Set<Message>()
        //     .Where(m => m.ChatRoomId == chatId)
        //     .ToListAsync();

        //    if (messages.Any())
        //    {
        //        _chatContext.Set<Message>().RemoveRange(messages);
        //        await _chatContext.SaveChangesAsync();
        //    }
        //}

        public async Task DeleteChatRoomAsync(string chatId)
        {
            var chatRoom = await _chatContext.Set<ChatRoom>().FindAsync(chatId);
            if (chatRoom != null)
            {
                _chatContext.Set<ChatRoom>().Remove(chatRoom);
                await _chatContext.SaveChangesAsync();
            }
        }

        public async Task<GetByIdChatRoomDto> GetChatRoomByIdAsync(string chatRoomId)
        {
            var chatRoom = await _chatContext.Set<ChatRoom>()
                                .Where(room => room.ChatRoomId == chatRoomId)
                                .FirstOrDefaultAsync();

            if (chatRoom == null)
            {
                throw new KeyNotFoundException("Chat room not found.");
            }

            return _mapper.Map<GetByIdChatRoomDto>(chatRoom);
        }

        public async Task<List<GetChatRoomByUserIdDto>> GetChatsByUserId(string userId)
        {
            var chatRooms = await _chatContext.Set<ChatRoom>()
                .Where(room => room.Members.Any(member => member.UserId == userId))
                .ToListAsync();

            return _mapper.Map<List<GetChatRoomByUserIdDto>>(chatRooms);
        }

        public async Task UpdateChatRoomAsync(UpdateChatRoomDto updateChatRoomDto)
        {
            var room = await _chatContext.Set<ChatRoom>().FindAsync(updateChatRoomDto.ChatRoomId);

            if(room != null)
            {
                _mapper.Map(updateChatRoomDto, room);
                await _chatContext.SaveChangesAsync();
            }
        }


    }
}
