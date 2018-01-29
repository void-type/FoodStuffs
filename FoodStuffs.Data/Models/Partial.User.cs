using FoodStuffs.Model.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Data.Models
{
    public partial class User : IUser
    {
        ICollection<IRecipe> IUser.RecipesCreatedByUser
        {
            get => RecipeCreatedByUser.Select(x => (IRecipe)x).ToArray();
            set => RecipeCreatedByUser = value.Select(x => (Recipe)x).ToArray();
        }

        ICollection<IRecipe> IUser.RecipesModifiedByUser
        {
            get => RecipeModifiedByUser.Select(x => (IRecipe)x).ToArray();
            set => RecipeModifiedByUser = value.Select(x => (Recipe)x).ToArray();
        }
    }
}