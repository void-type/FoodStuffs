using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses;

namespace FoodStuffs.Model.Events.Recipes
{
    public class GetRecipeResponseLogger : FallibleEventLoggerAbstract<GetRecipeRequest, GetRecipeResponse>
    {
        public GetRecipeResponseLogger(ILogger<GetRecipeResponseLogger> logger) : base(logger) { }

        protected override void OnSuccess(GetRecipeRequest request, GetRecipeResponse response)
        {
            Logger.LogInformation("Responded with {ResponseType}. RecipeId: {RecipeId}",
                nameof(GetRecipeResponse),
                response.Id);

            base.OnSuccess(request, response);
        }
    }
}
