using FoodStuffs.Model.Events.GroceryDepartments.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryDepartments;

public class SaveGroceryDepartmentRequestValidator : RuleValidatorAbstract<SaveGroceryDepartmentRequest>
{
    public SaveGroceryDepartmentRequestValidator()
    {
        CreateRule(new Failure("Grocery Department must have a name.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Grocery Department name can't be longer than 450 characters.", "name"))
            .InvalidWhen(entity => entity.Name.Length > 450);
    }
}
