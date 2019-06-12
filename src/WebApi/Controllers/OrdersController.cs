using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using WebApi.Commands;
using WebApi.Queries;

namespace WebApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICreateOrderCommand _createOrderCommand;
        private readonly IGetOrderQuery _getOrdersQuery;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ICreateOrderCommand createOrderCommand, IGetOrderQuery getOrdersQuery, ILoggerFactory loggerFactory)
        {
            _createOrderCommand = createOrderCommand;
            _getOrdersQuery = getOrdersQuery;
            _logger = loggerFactory.CreateLogger<OrdersController>();
        }

        // GET api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            Log.Information($"Get all orders from db");
            _logger.LogInformation($"Get all orders from db");
            return await _getOrdersQuery.Get();
        }

        // GET api/orders/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            _logger.LogInformation($"Get order by id {id}");
            return "orders";
        }

        // POST api/orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            _logger.LogInformation($"Add new order {JsonConvert.SerializeObject(order)}");
            await _createOrderCommand.AddOrder(order);
            return Ok();
        }

        // PUT api/orders/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/orders/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
