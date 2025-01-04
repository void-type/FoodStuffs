using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class CategoriesWithRecipesSpecification : QuerySpecificationAbstract<Category>
{
    public CategoriesWithRecipesSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(Category.Recipes));
    }
}
