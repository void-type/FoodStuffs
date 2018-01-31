using Core.Model.Validation;
using FoodStuffs.Model.ViewModels;
using System.Linq;

namespace FoodStuffs.Model.Validation
{
    public class RecipeViewModelValidator : AbstractSimpleValidator<IRecipeViewModel>
    {
        protected override void RunRules(IRecipeViewModel entity)
        {
            Invalid("Name", "Please enter a name.")
                .When(() => string.IsNullOrWhiteSpace(entity.Name));

            Invalid("Ingredients", "Please enter ingredients.")
                .When(() => string.IsNullOrWhiteSpace(entity.Ingredients));

            Invalid("Directions", "Please enter directions.")
                .When(() => string.IsNullOrWhiteSpace(entity.Directions));

            Invalid("CookTimeMinutes", "Cook time must be positive.")
                .When(() => entity.CookTimeMinutes < 0)
                .Suppress(() => entity.CookTimeMinutes == null);

            Invalid("PrepTimeMinutes", "Prep time must be positive.")
                .When(() => entity.PrepTimeMinutes < 0)
                .Suppress(() => entity.PrepTimeMinutes == null);

            Invalid("Categories", "One or more categories is invalid.")
                .When(() => entity.Categories.Any(string.IsNullOrWhiteSpace));
        }
    }
}