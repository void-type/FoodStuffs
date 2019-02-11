using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using System.Linq;
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

            protected override IResult<UserMessageWithEntityId<int>> HandleSync(Request request)
            {
                return _data.Recipes.Stored
                    .GetById(request.Id)
                    .ToResult("Recipe not found.")
                    .TeeOnSuccess(RemoveCategoryRecipes)
                    .TeeOnSuccess(_data.Recipes.Remove)
                    .TeeOnSuccess(_data.SaveChanges)
                    .Select(recipe => UserMessageWithEntityId.Create("Recipe deleted.", recipe.Id));
            }

            private void RemoveCategoryRecipes(Recipe recipe)
            {
                _data.CategoryRecipes.Stored
                    .Where(cr => cr.RecipeId == recipe.Id)
                    .Tee(_data.CategoryRecipes.RemoveRange);
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
