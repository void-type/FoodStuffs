using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.Recipes;

public class GetRecipeHandler : EventHandlerAbstract<GetRecipeRequest, GetRecipeResponse>
{
    private readonly IFoodStuffsData _data;

    public GetRecipeHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override Task<IResult<GetRecipeResponse>> Handle(GetRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesByIdWithAllRelatedSpecification(request.Id);

        return _data.Recipes.Get(byId, cancellationToken)
            .ToResultAsync(new RecipeNotFoundFailure())
            .SelectAsync(r => new GetRecipeResponse(
               Id: r.Id,
               Name: r.Name,
               Directions: r.Directions,
               CookTimeMinutes: r.CookTimeMinutes,
               PrepTimeMinutes: r.PrepTimeMinutes,
               CreatedBy: r.CreatedBy,
               CreatedOn: r.CreatedOn,
               ModifiedBy: r.ModifiedBy,
               ModifiedOn: r.ModifiedOn,
               PinnedImageId: r.PinnedImageId,
               IsForMealPlanning: r.IsForMealPlanning,
               Categories: r.Categories
                .Select(c => c.Name)
                .OrderBy(n => n),
               Images: r.Images.Select(i => i.Id),
               Ingredients: r.Ingredients
                .Select(i => new GetRecipeResponseIngredient(i.Name, i.Quantity, i.Order, i.IsCategory))
                .OrderBy(i => i.Order)));
    }
}
