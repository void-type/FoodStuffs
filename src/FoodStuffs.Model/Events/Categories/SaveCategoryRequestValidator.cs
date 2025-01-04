using FoodStuffs.Model.Events.Categories.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.Categories;

public class SaveCategoryRequestValidator : RuleValidatorAbstract<SaveCategoryRequest>
{
    public SaveCategoryRequestValidator()
    {
        CreateRule(new Failure("Category must have a name.", "categoryName"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Category name can't be longer than 450 characters.", "categoryName"))
            .InvalidWhen(entity => entity.Name.Length > 450);
    }
}
