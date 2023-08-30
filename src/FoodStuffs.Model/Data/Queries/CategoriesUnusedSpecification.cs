using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;
public class CategoriesUnusedSpecification : QuerySpecificationAbstract<Category>
{
    public CategoriesUnusedSpecification()
    {
        AddCriteria(x => !x.Recipes.Any());
        AddInclude(nameof(Category.Recipes));
    }
}
