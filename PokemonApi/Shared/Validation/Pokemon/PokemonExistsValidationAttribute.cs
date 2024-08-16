using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonApi.Interface;

namespace PokemonApi.Shared.Validation.Pokemon
{
    public class PokemonExistsValidationAttribute : IAsyncActionFilter
    {
        private readonly IPokemonRepository _repo;
        public PokemonExistsValidationAttribute(IPokemonRepository repo)
        {
            _repo = repo;
        }

        async public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("pokeId", out var idVal) && idVal is int id)
            {
                if (!await _repo.IsPokemonExists(id))
                {
                    context.Result = new UnprocessableEntityObjectResult("pokemon doesn't exist");
                    return;
                }
                await next();
            }
        }
    }
}
