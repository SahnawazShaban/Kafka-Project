using KafkaOrderApp.Models;
using KafkaOrderApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Kafka settings
builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("Kafka"));

// Register services
builder.Services.AddControllers();
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddHostedService<KafkaConsumerService>();

var app = builder.Build();

// Configure middleware
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
