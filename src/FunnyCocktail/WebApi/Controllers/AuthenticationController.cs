using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/authentication/")]
    public class AuthenticationController(IAuthenticationService authenticationService) : Controller
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserRequest request, CancellationToken cancellationToken = default)
        {
            await _authenticationService.SignUpAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserRequest request, CancellationToken cancelToken = default) =>
            Ok(await _authenticationService.SignInAsync(request, cancelToken));

        [HttpPost("get-user/{token}/{type}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFromToken(string token, string type, CancellationToken cancellationToken = default) =>
            Ok(await _authenticationService.GetUserFromTokenAsync(token, type, cancellationToken));
    }
}
