using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace WebApi.Queries
{
    public interface IGetOrderQuery
    {
        Task<List<Order>> Get();
    }
}
