using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.EntityLayer.Entities
{
    public class Message
    {
        [Key]
        public string MessageId { get; set; } 
        public string UserId { get; set; } = null!; 
        public string ChatRoomId { get; set; } = null!; 
        public string Content { get; set; } = null!; // Mesaj içeriği
        public string? MediaUrl { get; set; }  // Görsel, video gibi medya
        public DateTime SentAt { get; set; }
        public IsRead IsRead { get; set; } = 0; // Mesaj okunmuş mu?
    }
     
    public enum IsRead
    {
        Sent,
        Received,
        Read
    }
}
