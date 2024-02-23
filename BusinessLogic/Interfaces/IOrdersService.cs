using Core.DTOs;

namespace Core.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<OrderDto> GetAllByUser(string userId);
        Task Create(string userId);
    }
}
