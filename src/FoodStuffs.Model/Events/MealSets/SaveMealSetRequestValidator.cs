using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.MealSets;

public class SaveMealSetRequestValidator : RuleValidatorAbstract<SaveMealSetRequest>
{
    public SaveMealSetRequestValidator()
    {
        CreateRule(new Failure("Please enter a name.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Meal set cannot be empty.", "recipes"))
            .InvalidWhen(entity => !entity.RecipeIds.Any());
    }
}
