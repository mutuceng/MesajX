using MesajX.ChatService.EntityLayer.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.EntityLayer.Entities
{
    public class DMGroup:Group
    {

        public override bool ValidateMemberCount()
        {
            return GroupMembers.Count == 2;
        }
    }
}
