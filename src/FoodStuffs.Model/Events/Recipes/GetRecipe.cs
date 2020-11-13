using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Recipes
{
    public static class GetRecipe
    {
        public class Handler : EventHandlerAbstract<Request, RecipeDto>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override Task<IResult<RecipeDto>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new RecipesByIdWithCategoriesAndImagesSpecification(request.Id);

                return _data.Recipes.Get(byId, cancellationToken)
                    .ToResultAsync(new RecipeNotFoundFailure())
                    .SelectAsync(r => new RecipeDto(
                       Id: r.Id,
                       Name: r.Name,
                       Ingredients: r.Ingredients,
                       Directions: r.Directions,
                       CookTimeMinutes: r.CookTimeMinutes,
                       PrepTimeMinutes: r.PrepTimeMinutes,
                       CreatedBy: r.CreatedBy,
                       CreatedOn: r.CreatedOn,
                       ModifiedBy: r.ModifiedBy,
                       ModifiedOn: r.ModifiedOn,
                       PinnedImageId: r.PinnedImageId,
                       Categories: r.CategoryRecipes.Select(cr => cr.Category.Name).OrderBy(n => n),
                       Images: r.Images.Select(i => i.Id)));
            }
        }

        public record Request(int Id);

        public record RecipeDto(
            int Id,
            string Name,
            string Ingredients,
            string Directions,
            int? CookTimeMinutes,
            int? PrepTimeMinutes,
            string CreatedBy,
            DateTime CreatedOn,
            string ModifiedBy,
            DateTime ModifiedOn,
            int? PinnedImageId,
            IEnumerable<string> Categories,
            IEnumerable<int> Images);

        public class Logger : FallibleEventLogger<Request, RecipeDto>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<RecipeDto> result)
            {
                Logger.Info($"RequestRecipeId: {request.Id}");
                base.OnBoth(request, result);
            }
        }
    }
}
