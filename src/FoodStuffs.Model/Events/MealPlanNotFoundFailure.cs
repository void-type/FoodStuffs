using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class MealPlanNotFoundFailure : Failure
{
    public MealPlanNotFoundFailure() : base(errorMessage: "Meal plan not found.", uiHandle: "mealPlanId")
    {
    }
}
