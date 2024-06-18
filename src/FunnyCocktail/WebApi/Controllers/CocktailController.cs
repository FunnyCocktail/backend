using Application.DTOS;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/cocktails/")]
    public class CocktailController(ICocktailService cocktailService) : Controller
    {
        private readonly ICocktailService _cocktailService = cocktailService;

        [HttpPost("create-cocktail")]
        [Authorize(Roles = AccessRoles.Everyone)]
        public async Task<IActionResult> Create([FromBody] CreateCocktailDTO create, CancellationToken cancellationToken = default) =>
            Ok(await _cocktailService.CreateCocktailAsync(create, cancellationToken));

        [HttpGet("get-cocktail-by-id")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid Id, CancellationToken cancellationToken = default) =>
            Ok(await _cocktailService.GetAsync(Id, cancellationToken));

        [HttpGet("get-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default) =>
            Ok(await _cocktailService.GetAllCocktailsAsync(cancellationToken));

        [HttpGet("get-by-ingredient-id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIngredientId(Guid ingredientId, CancellationToken cancellationToken = default) =>
            Ok(await _cocktailService.GetAllCocktailsByIngredientIdAsync(ingredientId, cancellationToken));

        [HttpGet("get-by-name")]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken = default) =>
            Ok(await _cocktailService.GetAllCocktailsByNameAsync(name, cancellationToken));
    }
}
