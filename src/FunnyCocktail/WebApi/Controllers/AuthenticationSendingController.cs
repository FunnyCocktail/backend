using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/authentication-sending/")]
    public class AuthenticationSendingController(IAuthenticationSendingService authenticationSendingService) : Controller
    {
        private readonly IAuthenticationSendingService _authenticationSendingService = authenticationSendingService;

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] string data, CancellationToken cancellationToken = default)
        {
            await _authenticationSendingService.ForgotPasswordAsync(data, cancellationToken);
            return Ok(data);
        }
    }
}
