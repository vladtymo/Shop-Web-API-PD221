using Core.DTOs;

namespace Core.Interfaces
{
    public interface IAccountsService
    {
        Task Register(RegisterModel model);
        Task Login(/*model*/);
        Task Logout();
    }
}
