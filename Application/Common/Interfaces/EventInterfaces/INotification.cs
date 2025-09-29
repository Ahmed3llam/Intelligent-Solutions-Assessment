using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.EventInterfaces
{
    public interface INotification
    {
        Task SendNotificationAsync(string topic, string title, string body);
    }
}
