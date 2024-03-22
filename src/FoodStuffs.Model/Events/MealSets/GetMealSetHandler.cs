using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealSets;

public class GetMealSetHandler : CustomEventHandlerAbstract<GetMealSetRequest, GetMealSetResponse>
{
    private readonly FoodStuffsContext _data;

    public GetMealSetHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override Task<IResult<GetMealSetResponse>> Handle(GetMealSetRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealSetsWithAllRelatedSpecification(request.Id);

        return _data.MealSets
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
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
