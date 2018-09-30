using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System.Linq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Message;

namespace FoodStuffs.Model.DomainEvents.Recipes
{
    public class DeleteRecipe
    {
        public class Handler : DomainEventAbstract<Request, PostSuccessUserMessage<int>>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            protected override Result<PostSuccessUserMessage<int>> HandleInternal(Request request)
            {
                var recipeToRemove = _data.Recipes.Stored
                    .WhereById(request.Id)
                    .FirstOrDefault();

                if (recipeToRemove == null)
                {
                    return Result.Fail<PostSuccessUserMessage<int>>("Recipe not found.");
                }

                var categoryRecipesToRemove = _data.CategoryRecipes.Stored.WhereForRecipe(recipeToRemove.Id);

                _data.CategoryRecipes.RemoveRange(categoryRecipesToRemove);
                _data.Recipes.Remove(recipeToRemove);

                _data.SaveChanges();

                return Result.Ok(PostSuccessUserMessage.Create<int>("Recipe deleted.", request.Id));
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

        public class Logging : PostSuccessUserMessageLogging<Request, int>
        {
            public Logging(ILoggingService logger) : base(logger) { }

            public override void OnBoth(Request request, IResult<PostSuccessUserMessage<int>> result)
            {
                Logger.Info($"Id: '{request.Id}'");
                base.OnBoth(request, result);
            }
        }
    }
}
