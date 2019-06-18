using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Queries
{
    public class GetOrdersQuery : IGetOrderQuery
    {
        private readonly ShopContext _shopContext;

        public GetOrdersQuery(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<List<Order>> Get()
        {
            return await _shopContext.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            Thread.Sleep(1234);
            return await _shopContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Number.ToString().Contains(id.ToString()));
        }
    }
}
