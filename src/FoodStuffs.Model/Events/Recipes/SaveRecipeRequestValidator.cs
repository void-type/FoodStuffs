using FoodStuffs.Model.Events.Recipes.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipeRequestValidator : RuleValidatorAbstract<SaveRecipeRequest>
{
    public SaveRecipeRequestValidator()
    {
        CreateRule(new Failure("Name is required.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Grocery items quantity must be 1 or greater.", "groceryItems"))
            .InvalidWhen(entity => entity.GroceryItems?.Exists(i => i.Quantity <= 0) ?? false);

        CreateRule(new Failure("Side count must be 0 or greater.", "mealPlanningSidesCount"))
            .InvalidWhen(entity => entity.MealPlanningSidesCount < 0);

        CreateRule(new Failure("Cook time must be 0 or greater.", "cookTimeMinutes"))
            .InvalidWhen(entity => entity.CookTimeMinutes < 0);

        CreateRule(new Failure("Prep time must be 0 or greater.", "prepTimeMinutes"))
            .InvalidWhen(entity => entity.PrepTimeMinutes < 0);

        CreateRule(new Failure("Category names can't be longer than 450 characters.", "categories"))
            .InvalidWhen(entity => entity.Categories?.Exists(x => x.Length > 450) ?? false);
    }
}
