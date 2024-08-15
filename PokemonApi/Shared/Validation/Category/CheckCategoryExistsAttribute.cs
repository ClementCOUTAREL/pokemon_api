using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonApi.Interface;


namespace PokemonApi.Shared.Validation.Category
{
    public class CheckCategoryExistsAttribute : IAsyncActionFilter
    {
        private readonly ICategoryRepository _repo;

        public CheckCategoryExistsAttribute(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("catId", out var idVal) && idVal is int id)
            {
                if (!_repo.isCategoryExists(id))
                {
                    context.Result = new UnprocessableEntityObjectResult("category doesn't exist");
                    return;
                }
                await next();
            }
        }
    }
}
