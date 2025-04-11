using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.SyncService.Dtos
{
    public class SyncMessageDto
    {
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public string ChatRoomId { get; set; }
        public string Content { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime SentAt { get; set; }
    }
}
