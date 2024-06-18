using Application.DTOS;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/ingredients/")]
    public class IngredientController(IIngredientService ingredientService) : Controller
    {
        private readonly IIngredientService _ingredientService = ingredientService;

        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid Id, CancellationToken cancellationToken = default) =>
            Ok(await _ingredientService.GetAsync(Id, cancellationToken));

        [HttpGet("get-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default) =>
            Ok(await _ingredientService.GetAllAsync(cancellationToken));

        [HttpPost("create")]
        [Authorize(Roles = AccessRoles.Everyone)]
        public async Task<IActionResult> Create(CreateCocktailDTO create, CancellationToken cancellationToken = default)
        {
            await _ingredientService.CreateAsync(create, cancellationToken);
            return Ok();
        }
    }
}
