using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class GroceryDepartmentsWithAllRelatedSpecification : QuerySpecificationAbstract<GroceryDepartment>
{
    public GroceryDepartmentsWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(GroceryDepartment.GroceryItems));
    }
}
