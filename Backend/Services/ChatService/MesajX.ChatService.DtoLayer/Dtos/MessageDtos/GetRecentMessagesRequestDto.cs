using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DtoLayer.Dtos.MessageDtos
{
    public class GetRecentMessagesRequestDto
    {
        public string ChatRoomId { get; set; } = null!;
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
