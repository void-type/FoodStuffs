using FoodStuffs.Model.ViewModels;
using System.Linq;
using VoidCore.Model.Validation;

namespace FoodStuffs.Model.Validation
{
    public class RecipeViewModelValidator : ValidatorAbstract<IRecipeViewModel>
    {
        protected override void BuildRules()
        {
            CreateRule("Please enter a name.", "name")
                .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

            CreateRule("Please enter ingredients.", "ingredients")
                .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Ingredients));

            CreateRule("Please enter directions.", "directions")
                .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Directions));

            CreateRule("Cook time must be positive.", "cookTimeMinutes")
                .InvalidWhen(entity => entity.CookTimeMinutes < 0)
                .ExceptWhen(entity => entity.CookTimeMinutes == null);

            CreateRule("Prep time must be positive.", "prepTimeMinutes")
                .InvalidWhen(entity => entity.PrepTimeMinutes < 0)
                .ExceptWhen(entity => entity.PrepTimeMinutes == null);

            CreateRule("Category cannot be blank.", "categories")
                .InvalidWhen(entity => entity.Categories.Any(string.IsNullOrWhiteSpace));
        }
    }
}
