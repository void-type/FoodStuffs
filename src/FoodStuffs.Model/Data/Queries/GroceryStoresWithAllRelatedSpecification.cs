using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class GroceryStoresWithAllRelatedSpecification : QuerySpecificationAbstract<GroceryStore>
{
    public GroceryStoresWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(GroceryStore.GroceryItems));
    }
}
