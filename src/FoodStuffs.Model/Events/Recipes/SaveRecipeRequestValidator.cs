using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipeRequestValidator : RuleValidatorAbstract<SaveRecipeRequest>
{
    public SaveRecipeRequestValidator()
    {
        CreateRule(new Failure("Please enter a name.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Please enter ingredients.", "ingredients"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Ingredients));

        CreateRule(new Failure("Please enter directions.", "directions"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Directions));

        CreateRule(new Failure("Cook time must be positive.", "cookTimeMinutes"))
            .InvalidWhen(entity => entity.CookTimeMinutes < 0);

        CreateRule(new Failure("Prep time must be positive.", "prepTimeMinutes"))
            .InvalidWhen(entity => entity.PrepTimeMinutes < 0);
    }
}
