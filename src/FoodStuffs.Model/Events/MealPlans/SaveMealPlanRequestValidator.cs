using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;
using VoidCore.Model.Text;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanRequestValidator : RuleValidatorAbstract<SaveMealPlanRequest>
{
    public SaveMealPlanRequestValidator()
    {
        CreateRule(new Failure("Meal plan must have a name.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Pantry ingredients all need a name.", "pantryIngredients"))
            .InvalidWhen(entity => entity.PantryIngredients.Any(x => x.Name.IsNullOrWhiteSpace()));
    }
}
