using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesajX.ChatService.EntityLayer.Entities;

namespace MesajX.ChatService.EntityLayer.Entities.Abstract
{
    public abstract class Group
    {
        [Key]
        public string GroupId { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<GroupMember> GroupMembers { get; set; }
        public abstract bool ValidateMemberCount();
    }
}
