using MesajX.ChatService.DtoLayer.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos
{
    public class UpdateMemberDto
    {
        public string ChatRoomMemberId { get; set; }
        public string UserId { get; set; }
        public string ChatRoomId { get; set; }
        public Role Role { get; set; } 
    }
}
