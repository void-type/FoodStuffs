using System.Collections.Generic;

namespace FoodStuffs.Model.Interfaces.Services.Data.Models
{
    public interface IUser
    {
        string FirstName { get; set; }
        int Id { get; set; }
        bool IsAdmin { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        ICollection<IRecipe> RecipeCreatedByUser { get; set; }
        ICollection<IRecipe> RecipesModifiedByUser { get; set; }
        string Salt { get; set; }
        string UserName { get; set; }
    }
}