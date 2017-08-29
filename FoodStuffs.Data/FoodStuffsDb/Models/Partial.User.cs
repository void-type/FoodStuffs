using FoodStuffs.Model.Interfaces.Domain;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Models
{
    public partial class User : IUser
    {
        ICollection<IRecipe> IUser.RecipeCreatedByUser
        {
            get => (ICollection<IRecipe>)RecipeCreatedByUser;
            set => RecipeCreatedByUser = value.Select(x => (Recipe)x).ToArray();
        }

        ICollection<IRecipe> IUser.RecipeModifiedByUser
        {
            get => (ICollection<IRecipe>)RecipeModifiedByUser;
            set => RecipeModifiedByUser = value.Select(x => (Recipe)x).ToArray();
        }
    }
}