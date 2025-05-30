﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.EntityLayer.Entities
{
    public class ChatRoom
    {
        public string ChatRoomId { get; set; } = null!;
        public string? Name { get; set; } // Grup sohbetlerinde zorunlu, DM’lerde null
        public string? Photo { get; set; } // Grup sohbetlerinde opsiyonel, DM’lerde null
        public bool IsGroup { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<ChatRoomMember> Members { get; set; }
    }
}
