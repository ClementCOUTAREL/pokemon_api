using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokemonApi.Interface;

namespace PokemonApi.Shared.Validation.Reviewer
{
    public class ReviewerExistsValidationFilter : IAsyncActionFilter
    {
        private readonly IReviewerRepository _repo;
        public ReviewerExistsValidationFilter(IReviewerRepository repo)
        {
            _repo = repo;
        }

        async public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("reviewerId", out var idVal) && idVal is int id)
            {
                if (!await _repo.isReviewerExists(id))
                {
                    context.Result = new UnprocessableEntityObjectResult("reviewer doesn't exist");
                    return;
                }
                await next();
            }
        }
    }
}
