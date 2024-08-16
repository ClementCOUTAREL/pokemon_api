using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonApi.Interface;


namespace PokemonApi.Shared.Validation.Category
{
    public class CategoryExistsAttribute : IAsyncActionFilter
    {
        private readonly ICategoryRepository _repo;

        public CategoryExistsAttribute(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("categoryId", out var idVal) && idVal is int id)
            {
                if (!await _repo.isCategoryExists(id))
                {
                    context.Result = new UnprocessableEntityObjectResult("category doesn't exist");
                    return;
                }
                await next();
            }
        }
    }
}
