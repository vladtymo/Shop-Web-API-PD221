using Core.DTOs;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            this.accountsService = accountsService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            await accountsService.Register(model);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            return Ok(await accountsService.Login(model));
        }

        [HttpPost("refresh-tokens")]
        public async Task<IActionResult> RefreshTokens(UserTokens tokens)
        {
            return Ok(await accountsService.RefreshTokens(tokens));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutModel model)
        {
            await accountsService.Logout(model.RefreshToken);
            return Ok();
        }

        [HttpPost("google-sign-in")]
        public async Task<IActionResult> GoogleSignIn(GoogleSignInDto model)
        {
            return Ok(await accountsService.SignInWithGoogle(model));
        }
    }
}
