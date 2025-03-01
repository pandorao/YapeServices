using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapeServices.Ports.Messenger
{
    public interface IMessengerProducerService
    {
        Task SendMessageAsync(string key, string message);
    }
}
