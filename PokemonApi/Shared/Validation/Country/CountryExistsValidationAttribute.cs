using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonApi.Interface;

namespace PokemonApi.Shared.Validation.Country
{
    public class CountryExistsValidationAttribute : IAsyncActionFilter
    {
        private readonly ICountryRepository _repo;
        public CountryExistsValidationAttribute(ICountryRepository repo)
        {
            _repo = repo;
        }

        async public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("countryId", out var idVal) && idVal is int id)
            {
                if (!await _repo.isCountryExists(id))
                {
                    context.Result = new UnprocessableEntityObjectResult("country doesn't exist");
                    return;
                }
                await next();
            }
        }
    }
}
