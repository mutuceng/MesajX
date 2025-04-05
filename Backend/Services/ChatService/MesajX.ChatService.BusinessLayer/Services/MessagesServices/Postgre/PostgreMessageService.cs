using AutoMapper;
using MesajX.ChatService.DataAccessLayer.Concrete;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.MessagesServices.Postgre
{
    public class PostgreMessageService : IPostgreMessageService
    {
        private readonly ChatContext _chatContext;
        private readonly IMapper _mapper;

        public PostgreMessageService(IMapper mapper, ChatContext chatContext)
        {
            _mapper = mapper;
            _chatContext = chatContext;
        }

        public async Task<List<GetMessagesDto>> GetMessagesByRoomIdAsync(string roomId, int count)
        {
            var messages = await _chatContext.Set<Message>()
                .Where(m => m.ChatRoomId == roomId)
                .OrderByDescending(m => m.SentAt)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<List<GetMessagesDto>>(messages);
        }

        public async Task SaveMessagesAsync(List<SendMessageDto> sendMessageDtos, CancellationToken cancellationToken=default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var messages = new List<Message>();

            foreach (var msg in sendMessageDtos)
            {
                cancellationToken.ThrowIfCancellationRequested(); // Her bir öğe işlenmeden önce kontrol

                var message = _mapper.Map<Message>(msg);
                messages.Add(message);
            }
            //var messages = _mapper.Map<List<Message>>(sendMessageDtos);
            //foreach (var message in messages)
            //{
            //    message.SentAt = DateTime.UtcNow; // Set the sent time to now
            //    await _chatContext.Set<Message>().AddAsync(message);
            //}

            await _chatContext.Set<Message>().AddRangeAsync(messages);
            await _chatContext.SaveChangesAsync(cancellationToken);
        }
    }
}
