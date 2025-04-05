using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos
{
    public class GetChatRoomByUserIdDto
    {
        public string ChatRoomId { get; set; }
        public string? Name { get; set; }
        public string? Photo { get; set; }
    }
}
