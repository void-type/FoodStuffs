using System;

namespace FoodStuffs.Model.Data.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Blob Blob { get; set; }
    }
}
