using Core.DTOs;

namespace Core.Interfaces
{
    public interface IAccountsService
    {
        Task Register(RegisterModel model);
        Task Login(LoginModel model);
        Task Logout();
        //Task<ResetPasswordResponse> ResetPasswordRequest(string email);
        //Task ResetPassword(ResetPasswordModel model);
    }

    // TODO: implement reset user password endpoints
    public class ResetPasswordResponse
    {
        public string Token { get; set; }
    }
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
