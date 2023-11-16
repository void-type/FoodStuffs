using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealSets;

public class GetMealSetHandler : EventHandlerAbstract<GetMealSetRequest, GetMealSetResponse>
{
    private readonly IFoodStuffsData _data;

    public GetMealSetHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override Task<IResult<GetMealSetResponse>> Handle(GetMealSetRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealSetsByIdWithAllRelatedSpecification(request.Id);

        return _data.MealSets.Get(byId, cancellationToken)
            .ToResultAsync(new MealSetNotFoundFailure())
            .SelectAsync(r => new GetMealSetResponse(
                Id: r.Id,
                Name: r.Name,
                CreatedBy: r.CreatedBy,
                CreatedOn: r.CreatedOn,
                ModifiedBy: r.ModifiedBy,
                ModifiedOn: r.ModifiedOn,
                PantryIngredients: r.PantryIngredients.Select(i => new GetMealSetResponsePantryIngredient(i.Name, i.Quantity)),
                Recipes: r.Recipes.Select(r => new GetMealSetResponseRecipe(
                    Id: r.Id,
                    Name: r.Name,
                    Image: r.DefaultImage?.FileName,
                    Categories: r.Categories
                        .Select(c => c.Name)
                        .OrderBy(n => n),
                    Ingredients: r.Ingredients
                        .Select(i => new GetMealSetResponseRecipeIngredient(i.Name, i.Quantity, i.Order, i.IsCategory))
                        .OrderBy(i => i.Order)))));
    }
}
