using FoodStuffs.Model.Events.MealPlans.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanRequestValidator : RuleValidatorAbstract<SaveMealPlanRequest>
{
    public SaveMealPlanRequestValidator()
    {
        CreateRule(new Failure("Name is required.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Pantry shopping items quantity must 1 or greater.", "excludedShoppingItemsQuantity"))
            .InvalidWhen(entity => entity.ExcludedShoppingItems?.Exists(x => x.Quantity <= 0) ?? false);
    }
}
