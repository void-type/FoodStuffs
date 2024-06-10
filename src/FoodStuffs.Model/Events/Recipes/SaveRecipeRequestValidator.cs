using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipeRequestValidator : RuleValidatorAbstract<SaveRecipeRequest>
{
    public SaveRecipeRequestValidator()
    {
        CreateRule(new Failure("Please enter a name.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Please enter a name for all ingredients.", "ingredients"))
            .InvalidWhen(entity => entity.Ingredients.Exists(i => string.IsNullOrWhiteSpace(i.Name)));

        CreateRule(new Failure("Please enter a quantity greater than 1 for all ingredients.", "ingredients"))
            .InvalidWhen(entity => entity.Ingredients.Exists(i => i.Quantity < 1));

        CreateRule(new Failure("Cook time must be positive.", "cookTimeMinutes"))
            .InvalidWhen(entity => entity.CookTimeMinutes < 0);

        CreateRule(new Failure("Prep time must be positive.", "prepTimeMinutes"))
            .InvalidWhen(entity => entity.PrepTimeMinutes < 0);
    }
}
