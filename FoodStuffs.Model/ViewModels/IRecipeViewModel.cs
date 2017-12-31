using System;

namespace FoodStuffs.Model.ViewModels
{
    public interface IRecipeViewModel : ICreateRecipeViewModel
    {
        int CreatedByUserId { get; set; }
        DateTime CreatedOn { get; set; }
        int Id { get; set; }
        int ModifiedByUserId { get; set; }
        DateTime ModifiedOn { get; set; }
    }
}