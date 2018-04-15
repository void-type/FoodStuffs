using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public class User
    {
        public string FirstName { get; set; }

        public int Id { get; set; }

        public bool IsAdmin { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public ICollection<Recipe> RecipeCreatedByUser { get; set; }

        public ICollection<Recipe> RecipeModifiedByUser { get; set; }

        public string Salt { get; set; }

        public string UserName { get; set; }

        public User()
        {
            RecipeCreatedByUser = new HashSet<Recipe>();
            RecipeModifiedByUser = new HashSet<Recipe>();
        }
    }
}