using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class RecipesSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesSpecification(string name)
    {
        AddCriteria(r => r.Name == name);
    }

    public RecipesSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
    }

    public RecipesSpecification(IEnumerable<int> ids)
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
