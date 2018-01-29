using Core.Model.Validation;
using FoodStuffs.Model.Interfaces.Data.Models;

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