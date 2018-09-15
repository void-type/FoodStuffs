using FoodStuffs.Model.ViewModels;
using System.Linq;
using VoidCore.Model.Validation;

namespace FoodStuffs.Model.Validation
{
    public class RecipeViewModelValidator : AbstractValidator<IRecipeViewModel>
    {
        protected override void Rules(IRecipeViewModel entity)
        {
            Invalid("name", "Please enter a name.")
                .When(() => string.IsNullOrWhiteSpace(entity.Name));

            Invalid("ingredients", "Please enter ingredients.")
                .When(() => string.IsNullOrWhiteSpace(entity.Ingredients));

            Invalid("directions", "Please enter directions.")
                .When(() => string.IsNullOrWhiteSpace(entity.Directions));

            Invalid("cookTimeMinutes", "Cook time must be positive.")
                .When(() => entity.CookTimeMinutes < 0)
                .ExceptWhen(() => entity.CookTimeMinutes == null);

            Invalid("prepTimeMinutes", "Prep time must be positive.")
                .When(() => entity.PrepTimeMinutes < 0)
                .ExceptWhen(() => entity.PrepTimeMinutes == null);

            Invalid("categories", "Category cannot be blank.")
                .When(() => entity.Categories.Any(string.IsNullOrWhiteSpace));
        }
    }
}
