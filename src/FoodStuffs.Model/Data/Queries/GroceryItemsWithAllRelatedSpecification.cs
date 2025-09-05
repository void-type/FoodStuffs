using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class GroceryItemsWithAllRelatedSpecification : QuerySpecificationAbstract<GroceryItem>
{
    public GroceryItemsWithAllRelatedSpecification()
    {
        AddOrderBy(r => r.Id);

        AddInclude(nameof(GroceryItem.Recipes));
        AddInclude(nameof(GroceryItem.GroceryAisle));
        AddInclude(nameof(GroceryItem.StorageLocations));
    }

    public GroceryItemsWithAllRelatedSpecification(int id) : this()
    {
        AddCriteria(r => r.Id == id);
    }

    public GroceryItemsWithAllRelatedSpecification(IEnumerable<int> ids) : this()
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
