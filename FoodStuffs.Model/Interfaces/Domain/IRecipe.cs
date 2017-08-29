using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.Interfaces.Domain
{
    public interface IRecipe
    {
        int Id { get; set; }
        string Name { get; set; }
        string Directions { get; set; }
        string Ingredients { get; set; }
        int? PrepTimeMinutes { get; set; }
        int? CookTimeMinutes { get; set; }
        int CreatedByUserId { get; set; }
        int ModifiedByUserId { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }

        ICollection<ICategoryRecipe> CategoryRecipe { get; set; }
        IUser CreatedByUser { get; set; }
        IUser ModifiedByUser { get; set; }
    }
}