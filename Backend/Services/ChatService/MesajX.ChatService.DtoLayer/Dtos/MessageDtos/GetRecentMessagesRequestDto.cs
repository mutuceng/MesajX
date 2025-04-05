using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DtoLayer.Dtos.MessageDtos
{
    public class GetRecentMessagesRequestDto
    {
        public string ChatRoomId { get; set; }
        public int Count { get; set; }
        public DateTime? LastMessageDate { get; set; }
    }
}
