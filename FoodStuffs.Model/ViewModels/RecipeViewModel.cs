using System;

namespace FoodStuffs.Model.ViewModels
{
    public class RecipeViewModel : CreateRecipeViewModel, IRecipeViewModel
    {
        public int CreatedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Id { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}