using Core.DTOs;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<User> GoogleSignIn(GoogleSignInDto model);
    }
}
