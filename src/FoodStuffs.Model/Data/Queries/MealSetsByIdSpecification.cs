using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealSetsByIdSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsByIdSpecification(int id) : base()
    {
        AddCriteria(r => r.Id == id);
    }

    public MealSetsByIdSpecification(IEnumerable<int> ids) : base()
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
