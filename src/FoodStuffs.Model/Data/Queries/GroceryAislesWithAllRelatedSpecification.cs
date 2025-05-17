using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class GroceryAislesWithAllRelatedSpecification : QuerySpecificationAbstract<GroceryAisle>
{
    public GroceryAislesWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(GroceryAisle.GroceryItems));
    }
}
