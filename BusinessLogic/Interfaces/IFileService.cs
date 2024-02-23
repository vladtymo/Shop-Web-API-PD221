using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveProductImage(IFormFile file);
        Task DeleteProductImage(string path);
    }
}
