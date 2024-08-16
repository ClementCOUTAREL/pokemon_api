using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonApi.Interface;

namespace PokemonApi.Shared.Validation.Review
{
    public class ReviewExistsValidationAttribute : IAsyncActionFilter
    {
        private readonly IReviewRepository _repo;
        public ReviewExistsValidationAttribute(IReviewRepository repo)
        {
            _repo = repo;
        }

        async public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("reviewId", out var idVal) && idVal is int id)
            {
                if (!await _repo.isReviewExists(id))
                {
                    context.Result = new UnprocessableEntityObjectResult("review doesn't exist");
                    return;
                }
                await next();
            }
        }
    }
}
