using System.Threading.Tasks;
using Domain;

namespace WebApi.Commands
{
    public interface ICreateOrderCommand
    {
        Task AddOrder(Order order);
    }
}
