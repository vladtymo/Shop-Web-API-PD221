using Core.DTOs;
using Core.Models;

namespace Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductDto>> GetAll();
        Task<IEnumerable<ProductDto>> Get(IEnumerable<int> ids);
        Task<ProductDto?> Get(int id);
        int GetCount();
        IEnumerable<CategoryDto> GetAllCategories();
        void Create(CreateProductModel model);
        void Edit(EditProductModel model);
        void Delete(int id);
    }
}
