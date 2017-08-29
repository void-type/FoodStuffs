using FoodStuffs.Model.Interfaces.Domain;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Models
{
    public partial class User : IUser
    {
        ICollection<IRecipe> IUser.RecipeCreatedByUser
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