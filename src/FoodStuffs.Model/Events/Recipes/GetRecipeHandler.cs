﻿using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.Recipes
{
    public class GetRecipeHandler : EventHandlerAbstract<GetRecipeRequest, GetRecipeResponse>
    {
        private readonly IFoodStuffsData _data;

        public GetRecipeHandler(IFoodStuffsData data)
        {
            _data = data;
        }

        public override Task<IResult<GetRecipeResponse>> Handle(GetRecipeRequest request, CancellationToken cancellationToken = default)
        {
            var byId = new RecipesByIdWithCategoriesAndImagesSpecification(request.Id);

            return _data.Recipes.Get(byId, cancellationToken)
                .ToResultAsync(new RecipeNotFoundFailure())
                .SelectAsync(r => new GetRecipeResponse(
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
}
