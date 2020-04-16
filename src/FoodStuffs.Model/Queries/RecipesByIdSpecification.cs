using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Queries
{
    public class RecipesByIdSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesByIdSpecification(int id) : base()
        {
            AddCriteria(r => r.Id == id);
        }
    }
}
