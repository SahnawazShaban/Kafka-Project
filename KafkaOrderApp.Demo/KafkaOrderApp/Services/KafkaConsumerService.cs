using Confluent.Kafka;
using KafkaOrderApp.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace KafkaOrderApp.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _topic;
        private readonly ConsumerConfig _config;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(IOptions<KafkaConfig> kafkaConfig, ILogger<KafkaConsumerService> logger)
        {
            var config = kafkaConfig.Value;
            _topic = config.Topic;
            _config = new ConsumerConfig
            {
                BootstrapServers = config.BootstrapServers,
                GroupId = config.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(stoppingToken);
                var order = JsonSerializer.Deserialize<Order>(result.Message.Value);
                _logger.LogInformation($"Consumed order: {JsonSerializer.Serialize(order)}");
                await Task.Delay(1000); // Simulate work
            }
        }
    }
}
