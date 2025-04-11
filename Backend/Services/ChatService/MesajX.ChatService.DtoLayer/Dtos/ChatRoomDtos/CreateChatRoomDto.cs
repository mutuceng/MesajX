using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos
{
    public class CreateChatRoomDto
    {
        public string ChatRoomId { get; set; } = null!;
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public bool IsGroup { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<string> Members { get; set; } // Üye listesi

        public bool ValidateMemberCount()
        {
            if (!IsGroup)
                return Members.Count == 2; // DM’ler için tam 2 üye
            return Members.Count >= 1; // Grup sohbetleri için en az 1 üye
        }
    }
}
