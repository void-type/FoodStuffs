using System.Collections.Generic;

namespace FoodStuffs.Model.Interfaces.Domain
{
    public interface IUser
    {
        int Id { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool IsAdmin { get; set; }
        string Salt { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        ICollection<IRecipe> RecipeCreatedByUser { get; set; }
        ICollection<IRecipe> RecipeModifiedByUser { get; set; }
    }
}