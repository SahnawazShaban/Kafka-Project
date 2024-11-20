using Confluent.Kafka;
using KafkaOrderApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace KafkaOrderApp.Services
{
    public class KafkaProducerService
    {
        private readonly string _topic;
        private readonly ProducerConfig _config;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IOptions<KafkaConfig> kafkaConfig, ILogger<KafkaProducerService> logger)
        {
            var config = kafkaConfig.Value;
            _topic = config.Topic;
            _config = new ProducerConfig { BootstrapServers = config.BootstrapServers };
            _logger = logger;
        }

        public async Task ProduceAsync(Order order)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(order) };
            await producer.ProduceAsync(_topic, message);
        }
    }
}
