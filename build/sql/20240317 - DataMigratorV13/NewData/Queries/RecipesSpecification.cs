using DataMigratorV13.NewData.Models;
using VoidCore.Model.Data;

namespace DataMigratorV13.NewData.Queries;

public class RecipesSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
    }

    public RecipesSpecification(IEnumerable<int> ids)
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
