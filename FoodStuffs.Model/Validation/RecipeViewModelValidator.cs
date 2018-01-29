using Core.Model.Validation;
using FoodStuffs.Model.ViewModels;
using System.Linq;

namespace FoodStuffs.Model.Validation
{
    public class RecipeViewModelValidator : AbstractSimpleValidator<IRecipeViewModel>
    {
        protected override void SetRules(IRecipeViewModel entity)
        {
            InValid("Name", "Please enter a name.")
                .When(() => string.IsNullOrWhiteSpace(entity.Name));

            InValid("Ingredients", "Please enter ingredients.")
                .When(() => string.IsNullOrWhiteSpace(entity.Ingredients));

            InValid("Directions", "Please enter directions.")
                .When(() => string.IsNullOrWhiteSpace(entity.Directions));

            InValid("CookTimeMinutes", "Cook time must be positive.")
                .When(() => entity.CookTimeMinutes < 0)
                .Suppress(() => entity.CookTimeMinutes == null);

            InValid("PrepTimeMinutes", "Prep time must be positive.")
                .When(() => entity.PrepTimeMinutes < 0)
                .Suppress(() => entity.PrepTimeMinutes == null);

            InValid("Categories", "One or more categories is invalid.")
                .When(() => entity.Categories.Any(string.IsNullOrWhiteSpace));
        }
    }
}