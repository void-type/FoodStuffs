using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Events.MealPlans.Models;

public static class MealPlanEventModelMappers
{
    public static GetMealPlanResponse ToGetMealPlanResponse(this MealPlan m)
    {
        return new GetMealPlanResponse(
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
                    mealPlanningSidesCount: rel.Recipe.MealPlanningSidesCount,
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
                                GroceryAisleId: i.GroceryItem.GroceryAisle?.Id)
                        )
                        .OrderBy(i => i.Order)]))
                .OrderBy(rel => rel.Order)]);
    }
}
