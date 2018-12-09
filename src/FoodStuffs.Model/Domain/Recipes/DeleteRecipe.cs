using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Domain.Recipes
{
    public class DeleteRecipe
    {
        public class Handler : EventHandlerSyncAbstract<Request, UserMessageWithEntityId<int>>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            protected override Result<UserMessageWithEntityId<int>> HandleSync(Request request)
            {
                var maybeRecipe = _data.Recipes.Stored.GetById(request.Id);

                if (maybeRecipe.HasNoValue)
                {
                    return Result.Fail<UserMessageWithEntityId<int>>("Recipe not found.");
                }

                var recipeToRemove = maybeRecipe.Value;

                var categoryRecipesToRemove = _data.CategoryRecipes.Stored
                    .WhereForRecipe(recipeToRemove.Id);

                _data.CategoryRecipes.RemoveRange(categoryRecipesToRemove);
                _data.Recipes.Remove(recipeToRemove);

                _data.SaveChanges();

                return Result.Ok(UserMessageWithEntityId.Create("Recipe deleted.", request.Id));
            }

            private readonly IFoodStuffsData _data;
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

            public override void OnBoth(Request request, IResult<UserMessageWithEntityId<int>> result)
            {
                Logger.Info($"Id: '{request.Id}'");
                base.OnBoth(request, result);
            }
        }
    }
}
