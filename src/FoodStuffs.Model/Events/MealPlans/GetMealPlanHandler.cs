using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealPlans;

public class GetMealPlanHandler : CustomEventHandlerAbstract<GetMealPlanRequest, GetMealPlanResponse>
{
    private readonly FoodStuffsContext _data;

    public GetMealPlanHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetMealPlanResponse>> Handle(GetMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealPlansWithAllRelatedSpecification(request.Id);

        return await _data.MealPlans
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new MealPlanNotFoundFailure())
            .SelectAsync(m => new GetMealPlanResponse(
                Id: m.Id,
                Name: m.Name,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                PantryShoppingItems: m.PantryShoppingItemRelations
                    .ConvertAll(i =>
                        new GetMealPlanResponsePantryShoppingItem(
                            Id: i.ShoppingItem.Id,
                            Name: i.ShoppingItem.Name,
                            Quantity: i.Quantity)
                    ),
                Recipes: m.RecipeRelations
                    .ConvertAll(rel => new GetMealPlanResponseRecipe(
                        Id: rel.Recipe.Id,
                        Name: rel.Recipe.Name,
                        Order: rel.Order,
                        Image: rel.Recipe.DefaultImage?.FileName,
                        Categories: rel.Recipe.Categories
                            .ConvertAll(c => c.Name)
                            .ToList(),
                        ShoppingItems: rel.Recipe.ShoppingItemRelations
                            .OrderBy(i => i.Order)
                            .Select(i =>
                                new GetMealPlanResponseRecipeShoppingItem(
                                    Id: i.ShoppingItem.Id,
                                    Name: i.ShoppingItem.Name,
                                    Quantity: i.Quantity)
                            )
                            .ToList()))));
    }
}
