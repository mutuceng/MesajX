using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos
{
    public class UpdateChatRoomDto
    {
        public string ChatRoomId { get; set; }
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
