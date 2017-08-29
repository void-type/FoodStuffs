using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Validation.Core;

namespace FoodStuffs.Model.Validation
{
    public class RecipeValidator : AbstractSimpleValidator<IRecipe>
    {
        protected override void SetRules(IRecipe entity)
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