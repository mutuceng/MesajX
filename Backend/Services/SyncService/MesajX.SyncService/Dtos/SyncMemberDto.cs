using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.SyncService.Dtos
{
    public class SyncMemberDto
    {
        public string UserId { get; set; }
        public string ChatRoomId { get; set; }
        public Role? Role { get; set; }
    }


}
