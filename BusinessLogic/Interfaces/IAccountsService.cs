using Core.DTOs;
using Core.Utilities;

namespace Core.Interfaces
{
    public class JwtResponseVM
    {
        public string Token { get; set; }
    }
    public interface IAccountsService
    {
        Task Register(RegisterModel model);
        Task<LoginResponseDto> Login(LoginModel model);
        Task<UserTokens> RefreshTokens(UserTokens tokens);
        Task Logout(string refreshToken);

        Task RemoveExpiredRefreshTokens();

        Task<LoginResponseDto> SignInWithGoogle(GoogleSignInDto model);
        //Task<BaseResponse<JwtResponseVM>> SignInWithFacebook(FacebookSignInDto model);

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
