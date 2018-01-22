using Core.Model.Validation;
using FoodStuffs.Model.ViewModels;

namespace FoodStuffs.Model.Validation
{
    public class RecipeValidator : AbstractSimpleValidator<IRecipeViewModel>
    {
        protected override void SetRules(IRecipeViewModel entity)
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