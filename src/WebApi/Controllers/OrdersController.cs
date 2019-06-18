using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using WebApi.Commands;
using WebApi.Queries;
using Order = Domain.Order;

namespace WebApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICreateOrderCommand _createOrderCommand;
        private readonly IGetOrderQuery _getOrdersQuery;
        private readonly ILogger<OrdersController> _logger;
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheEntryOptions;

        public OrdersController(ICreateOrderCommand createOrderCommand, IGetOrderQuery getOrdersQuery, ILoggerFactory loggerFactory, IDistributedCache cache)
        {
            _createOrderCommand = createOrderCommand;
            _getOrdersQuery = getOrdersQuery;
            _logger = loggerFactory.CreateLogger<OrdersController>();
            _cache = cache;
            _cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Http("http://localhost:5000")
                .CreateLogger();

        }

        // GET api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            Log.Information("Get all orders from db");
            return await _getOrdersQuery.Get();
        }

        // GET api/orders/5
        [HttpGet("{id}/cache")]
        public async Task<ActionResult<Order>> GetWithCache(int id)
        {
            Log.Information("Get order with cache");

            var cacheValue = _cache.Get($"orderid{id}");
            if (cacheValue != null)
            {
                var cacheString = Encoding.UTF8.GetString(cacheValue);

                Log.Information($"Get order from cache by id {id}");
               
                return JsonConvert.DeserializeObject<Order>(cacheString);
            }
            else
            {
                Log.Information($"Get order from database by id {id}");

                var data = await _getOrdersQuery.GetById(id);
                _cache.Set($"orderid{id}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)),
                    _cacheEntryOptions);

                Log.Information($"Set order to cache with id {id}");
             
                return data;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Log.Information($"Get order from database by id {id}");
           
            var a = await _getOrdersQuery.GetById(id);
            return a;
        }

        // POST api/orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            Log.Information($"Add new order {JsonConvert.SerializeObject(order)}");
         
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
