using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
using YapeServices.Ports.Messenger;

namespace YapeServices.Kafka
{
    public class KafkaProducerService : IMessengerProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _topic = kafkaSettings.Value.Topic;
        }

        public async Task SendMessageAsync(string key, string message)
        {
            await _producer.ProduceAsync(_topic, new Message<string, string>
            {
                Key = key,
                Value = message
            });
        }
    }
}
