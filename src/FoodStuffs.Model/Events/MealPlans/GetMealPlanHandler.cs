using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.MealPlans.Models;
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
                ExcludedGroceryItems: m.ExcludedGroceryItemRelations
                    .ConvertAll(i =>
                        new GetMealPlanResponseExcludedGroceryItem(
                            Id: i.GroceryItem.Id,
                            Quantity: i.Quantity)
                    ),
                Recipes: [.. m.RecipeRelations
                    .Select(rel => new GetMealPlanResponseRecipe(
                        Id: rel.Recipe.Id,
                        Name: rel.Recipe.Name,
                        Order: rel.Order,
                        IsComplete: rel.IsComplete,
                        Image: rel.Recipe.DefaultImage?.FileName,
                        Categories: [.. rel.Recipe.Categories
                            .Where(c => c.ShowInMealPlan)
                            .Select(c => new GetMealPlanResponseRecipeCategory(
                                Id: c.Id,
                                Name: c.Name,
                                Color: c.Color
                            ))
                            .OrderBy(c => c.Name)],
                        GroceryItems: [.. rel.Recipe.GroceryItemRelations
                            .Select(i =>
                                new GetMealPlanResponseRecipeGroceryItem(
                                    Id: i.GroceryItem.Id,
                                    Name: i.GroceryItem.Name,
                                    InventoryQuantity: i.GroceryItem.InventoryQuantity,
                                    Quantity: i.Quantity,
                                    Order: i.Order,
                                    GroceryAisleId: i.GroceryItem?.GroceryAisle?.Id)
                            )
                            .OrderBy(i => i.Order)]))
                    .OrderBy(rel => rel.Order)]));
    }
}
