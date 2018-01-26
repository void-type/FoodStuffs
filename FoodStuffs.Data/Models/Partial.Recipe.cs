using FoodStuffs.Model.Interfaces.Services.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Data.Models
{
    public partial class Recipe : IRecipe
    {
        ICollection<ICategoryRecipe> IRecipe.CategoryRecipe
        {
            get => CategoryRecipe.Select(x => (ICategoryRecipe)x).ToArray();
            set => CategoryRecipe = value.Select(x => (CategoryRecipe)x).ToArray();
        }

        IUser IRecipe.CreatedByUser
        {
            get => CreatedByUser;
            set => CreatedByUser = (User)value;
        }

        IUser IRecipe.ModifiedByUser
        {
            get => ModifiedByUser;
            set => ModifiedByUser = (User)value;
        }
    }
}