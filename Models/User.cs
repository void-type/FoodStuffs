using System;
using System.Collections.Generic;

namespace FoodStuffs.Data
{
    public partial class User
    {
        public User()
        {
            RecipeCreatedByUser = new HashSet<Recipe>();
            RecipeModifiedByUser = new HashSet<Recipe>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Recipe> RecipeCreatedByUser { get; set; }
        public ICollection<Recipe> RecipeModifiedByUser { get; set; }
    }
}
