using FoodStuffs.Model.Events.GroceryAisles.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryAisles;

public class SaveGroceryAisleRequestValidator : RuleValidatorAbstract<SaveGroceryAisleRequest>
{
    public SaveGroceryAisleRequestValidator()
    {
        CreateRule(new Failure("Name is required.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Name can't be longer than 450 characters.", "name"))
            .InvalidWhen(entity => entity.Name?.Length > 450);
    }
}
