using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/authentication-confirm/")]
    public class AuthenticationConfirmController(IAuthenticationConfirmService authenticationConfirmService) : Controller
    {
        private readonly IAuthenticationConfirmService _authenticationConfirmService = authenticationConfirmService;

        [HttpPut("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] Verification verification, CancellationToken cancellationToken = default) =>
            Ok(await _authenticationConfirmService.ConfirmEmailAsync(verification, cancellationToken));

        [HttpPost("update-password")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateData data, CancellationToken cancellationToken = default) =>
            Ok(await _authenticationConfirmService.UpdatePasswordAsync(data, cancellationToken));
    }
}
