using MesajX.ChatService.EntityLayer.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.EntityLayer.Entities
{
    public class TeamGroup: Group
    {
        public string TeamGroupName { get; set; } = null!;
        public string? TeamGroupPhoto { get; set; }

        public override bool ValidateMemberCount()
        {
            return true; // Herhangi bir sayıda üye olabilir
        }
    }
}
