using AutoMapper;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
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

            CreateMap<ChatRoom, GetByIdChatRoomDto>().ReverseMap();
            CreateMap<ChatRoom, GetChatRoomByUserIdDto>().ReverseMap();
            CreateMap<ChatRoom, CreateChatRoomDto>().ReverseMap();
            CreateMap<ChatRoom, UpdateChatRoomDto>().ReverseMap();

            CreateMap<ChatRoomMember, CreateMemberDto>().ReverseMap();
            CreateMap<ChatRoomMember, GetMembersByRoomIdDto>().ReverseMap();
            CreateMap<ChatRoomMember, UpdateMemberDto>().ReverseMap();




        }
    }
}
