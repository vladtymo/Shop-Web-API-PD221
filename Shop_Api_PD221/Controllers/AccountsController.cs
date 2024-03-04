using Core.DTOs;
using Core.Interfaces;
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

        [HttpPost("refreshTokens")]
        public IActionResult RefreshTokens(UserTokens tokens)
        {
            return Ok(accountsService.RefreshTokens(tokens));
        }

        [HttpPost("logout")]
        public IActionResult Logout(LogoutModel model)
        {
            accountsService.Logout(model.RefreshToken);
            return Ok();
        }
    }
}
