using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonApi.Interface;

namespace PokemonApi.Shared.Validation.Owner
{
    public class OwnerExistValidationAttribute : IAsyncActionFilter
    {
        private readonly IOwnerRepository _repo;
        public OwnerExistValidationAttribute(IOwnerRepository repo)
        {
            _repo = repo;
        }

        async public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("ownerId", out var idVal) && idVal is int id)
            {
                if (!await _repo.isOwnerExists(id))
                {
                    context.Result = new UnprocessableEntityObjectResult("owner doesn't exist");
                    return;
                }
                await next();
            }
        }
    }
}
