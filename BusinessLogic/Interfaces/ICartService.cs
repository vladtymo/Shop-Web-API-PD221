using Core.DTOs;

namespace Core.Interfaces
{
    public interface ICartService
    {
        IEnumerable<ProductDto> GetProducts();
        IEnumerable<int> GetProductIds();
        void Add(int id);
        void Remove(int id);
        int GetCount();
        bool IsExists(int id);
    }
}
