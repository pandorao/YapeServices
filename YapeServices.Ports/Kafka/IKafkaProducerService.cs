using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapeServices.Ports.Kafka
{
    public interface IKafkaProducerService
    {
        Task SendMessageAsync(string key, string message);
    }
}
