using FoodStuffs.Model.Validation.Core;
using FoodStuffs.Model.ViewModels;

namespace FoodStuffs.Model.Validation
{
    public class RecipeValidator : AbstractSimpleValidator<RecipeViewModel>
    {
        protected override void SetRules(RecipeViewModel entity)
        {
            Valid("Name", "Please enter a name.")
                .When(() => !string.IsNullOrWhiteSpace(entity.Name));

            Valid("Ingredients", "Please enter ingredients.")
                .When(() => !string.IsNullOrWhiteSpace(entity.Ingredients));

            Valid("Directions", "Please enter directions.")
                .When(() => !string.IsNullOrWhiteSpace(entity.Directions));
        }
    }
}