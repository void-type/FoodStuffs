using System.Collections.Generic;

namespace FoodStuffs.Model.Data.Models
{
    public partial class User
    {
        public string FirstName { get; set; }

        public int Id { get; set; }

        public bool IsAdmin { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Recipe> RecipesCreatedByUser { get; set; }

        public virtual ICollection<Recipe> RecipesModifiedByUser { get; set; }

        public string Salt { get; set; }

        public string UserName { get; set; }

        public User()
        {
            RecipesCreatedByUser = new HashSet<Recipe>();
            RecipesModifiedByUser = new HashSet<Recipe>();
        }
    }
}