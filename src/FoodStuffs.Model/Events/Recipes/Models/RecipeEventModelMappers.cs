using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Events.Recipes.Models;

public static class RecipeEventModelMappers
{
    public static GetRecipeResponse ToGetRecipeResponse(this Recipe r)
    {
        return new GetRecipeResponse(
            Id: r.Id,
            Name: r.Name,
            Directions: r.Directions,
            Sides: r.Sides,
            PrepTimeMinutes: r.PrepTimeMinutes,
            CookTimeMinutes: r.CookTimeMinutes,
            IsForMealPlanning: r.IsForMealPlanning,
            MealPlanningSidesCount: r.MealPlanningSidesCount,
            CreatedBy: r.CreatedBy,
            CreatedOn: r.CreatedOn,
            ModifiedBy: r.ModifiedBy,
            ModifiedOn: r.ModifiedOn,
            Slug: r.Slug,
            DefaultImage: r.DefaultImage?.FileName,
            PinnedImage: r.PinnedImage?.FileName,
            Images: r.Images
                .ConvertAll(i => i.FileName),
            Categories: [.. r.Categories
            .Select(c => new GetRecipeResponseCategory(
                Id: c.Id,
                Name: c.Name,
                Color: c.Color
            ))
            .OrderBy(c => c.Name)],
            GroceryItems: [.. r.GroceryItemRelations
            .Select(i => new GetRecipeResponseGroceryItem(
                Id: i.GroceryItem.Id,
                Name: i.GroceryItem.Name,
                InventoryQuantity: i.GroceryItem.InventoryQuantity,
                Quantity: i.Quantity,
                Order: i.Order))
            .OrderBy(i => i.Order)]
        );
    }
}
