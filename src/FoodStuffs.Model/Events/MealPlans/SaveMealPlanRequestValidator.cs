using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanRequestValidator : RuleValidatorAbstract<SaveMealPlanRequest>
{
    public SaveMealPlanRequestValidator()
    {
        CreateRule(new Failure("Meal plan must have a name.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Pantry shopping items quantity must be positive.", "pantryShoppingItemsQuantity"))
            .InvalidWhen(entity => entity.PantryShoppingItems.Exists(x => x.Quantity <= 0));
    }
}
