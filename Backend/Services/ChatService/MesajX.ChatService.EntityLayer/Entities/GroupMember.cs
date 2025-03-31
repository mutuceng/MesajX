using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.EntityLayer.Entities
{
    public class GroupMember
    {
        [Key]
        public string GroupMemberId { get; set; }  = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string GroupId { get; set; }
    }
}
