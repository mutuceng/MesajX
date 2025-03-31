using AutoMapper;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Mapping
{
    public class GeneralMapping: Profile
    {
        public GeneralMapping()
        {
            CreateMap<Message, SendMessageDto>().ReverseMap();
            CreateMap<Message, GetMessagesDto>().ReverseMap();
        }
    }
}
