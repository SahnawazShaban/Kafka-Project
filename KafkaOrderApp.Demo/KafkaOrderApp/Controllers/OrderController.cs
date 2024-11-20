using KafkaOrderApp.Models;
using KafkaOrderApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KafkaOrderApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly KafkaProducerService _producerService;

        public OrderController(KafkaProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _producerService.ProduceAsync(order);
            return Ok(new { Message = "Order sent to Kafka" });
        }
    }
}
