using Application.DTOS;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/rating/")]
    public class RatingController(IRatingService ratingService) : Controller
    {
        private readonly IRatingService _ratingService = ratingService;

        [HttpGet("get-all-grades")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGrades(Guid Id, CancellationToken cancellationToken = default) =>
            Ok(await _ratingService.GetAllGradesAsync(Id, cancellationToken));

        [HttpPost("rate-cocktail")]
        [Authorize(Roles = AccessRoles.User)]
        public async Task<IActionResult> Rate(RateCocktailDTO rate, CancellationToken cancellationToken = default) =>
            Ok(await _ratingService.RateCocktailAsync(rate, cancellationToken));
    }
}
