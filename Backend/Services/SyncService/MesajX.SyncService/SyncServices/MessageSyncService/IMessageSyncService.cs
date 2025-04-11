using MesajX.SyncService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.SyncService.SyncServices.MessageSyncService
{
    public interface IMessageSyncService
    {
        Task SaveMessageToPostgreAsync(SyncMessageDto messageDto);
    }
}
