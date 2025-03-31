using MesajX.ChatService.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DataAccessLayer.Concrete
{
    public class ChatContext:DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }
        DbSet<Message> Messages { get; set; }
        DbSet<GroupMember> GroupMembers { get; set; }
        DbSet<DMGroup> DMGroups { get; set; }
        DbSet<TeamGroup> TeamGroups { get; set; }

    }
}
