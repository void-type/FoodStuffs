using System.Collections.Generic;

namespace FoodStuffs.Data.Models
{
    public partial class User
    {
        public string FirstName { get; set; }

        public int Id { get; set; }

        public bool IsAdmin { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Recipe> RecipeCreatedByUser { get; set; }

        public virtual ICollection<Recipe> RecipeModifiedByUser { get; set; }

        public string Salt { get; set; }

        public string UserName { get; set; }

        public User()
        {
            RecipeCreatedByUser = new HashSet<Recipe>();
            RecipeModifiedByUser = new HashSet<Recipe>();
        }
    }
}