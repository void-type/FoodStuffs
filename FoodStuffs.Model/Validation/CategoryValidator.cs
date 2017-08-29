using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Validation.Core;

namespace FoodStuffs.Model.Validation
{
    public class CategoryValidator : AbstractSimpleValidator<ICategory>
    {
        protected override void SetRules(ICategory validatable)
        {
            Valid("Name", "Please enter a category name.")
                .When(() => !string.IsNullOrWhiteSpace(validatable.Name));
        }
    }
}