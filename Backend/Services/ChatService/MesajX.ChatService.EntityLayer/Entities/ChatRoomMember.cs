using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.EntityLayer.Entities
{
    public class ChatRoomMember
    {
        [Key]
        public string ChatRoomMemberId { get; set; }  = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string ChatRoomId { get; set; }
        public Role Role { get; set; } = Role.Member;
        public ChatRoom ChatRoom { get; set; }
    }

    public enum Role
    {
        Moderator,
        Member
    }
}
