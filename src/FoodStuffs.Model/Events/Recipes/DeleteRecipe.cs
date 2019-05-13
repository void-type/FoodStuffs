using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes
{
    public class DeleteRecipe
    {
        public class Handler : EventHandlerAbstract<Request, UserMessageWithEntityId<int>>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<UserMessageWithEntityId<int>>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new RecipesByIdWithCategoriesSpecification(request.Id);

                return await _data.Recipes.Get(byId, cancellationToken)
                    .ToResultAsync(new RecipeNotFoundFailure())
                    .TeeOnSuccessAsync(RemoveRecipe)
                    .SelectAsync(r => UserMessageWithEntityId.Create("Recipe deleted.", r.Id));
            }

            private async Task RemoveRecipe(Recipe recipe)
            {
                await _data.CategoryRecipes.RemoveRange(recipe.CategoryRecipe.AsReadOnly());
                await _data.Recipes.Remove(recipe);
            }
        }

        public class Request
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Logger : UserMessageWithEntityIdEventLogger<Request, int>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<UserMessageWithEntityId<int>> result)
            {
                Logger.Info($"Id: '{request.Id}'");
                base.OnBoth(request, result);
            }
        }
    }
}
