using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.Interfaces.Domain
{
    public interface IRecipe
    {
        ICollection<ICategoryRecipe> CategoryRecipe { get; set; }
        int? CookTimeMinutes { get; set; }
        IUser CreatedByUser { get; set; }
        int CreatedByUserId { get; set; }
        DateTime CreatedOn { get; set; }
        string Directions { get; set; }
        int Id { get; set; }
        string Ingredients { get; set; }
        IUser ModifiedByUser { get; set; }
        int ModifiedByUserId { get; set; }
        DateTime ModifiedOn { get; set; }
        string Name { get; set; }
        int? PrepTimeMinutes { get; set; }
    }
}