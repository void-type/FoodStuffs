using FoodStuffs.Model.Data.EntityFramework;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealSets;

public class GetMealSetHandler : EventHandlerAbstract<GetMealSetRequest, GetMealSetResponse>
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
            .ApplyEfSpecification(byId)
            .AsSplitQuery()
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
