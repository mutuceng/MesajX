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
        DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
        DbSet<ChatRoom> ChatRooms { get; set; }
    }
}
