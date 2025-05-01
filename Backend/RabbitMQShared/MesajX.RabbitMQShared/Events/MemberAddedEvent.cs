using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.RabbitMQShared.Events
{
    public class MemberAddedEvent
    {
        public string UserId { get; set; }
        public string ChatRoomId { get; set; }
        public Role? Role { get; set; }
    }

    public enum Role
    {
        Moderator,
        Member
    }
}
