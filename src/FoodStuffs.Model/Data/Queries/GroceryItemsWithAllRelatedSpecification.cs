using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class GroceryItemsWithAllRelatedSpecification : QuerySpecificationAbstract<GroceryItem>
{
    public GroceryItemsWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);

        AddCriteria(r => r.Id == id);
        AddInclude(nameof(GroceryItem.Recipes));
        AddInclude(nameof(GroceryItem.GroceryDepartment));
        AddInclude(nameof(GroceryItem.PantryLocations));
    }
}
