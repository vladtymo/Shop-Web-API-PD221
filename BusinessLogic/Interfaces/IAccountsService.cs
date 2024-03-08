using Core.DTOs;

namespace Core.Interfaces
{
    public interface IAccountsService
    {
        Task Register(RegisterModel model);
        Task<LoginResponseDto> Login(LoginModel model);
        Task<UserTokens> RefreshTokens(UserTokens tokens);
        Task Logout(string refreshToken);

        Task RemoveExpiredRefreshTokens();

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
