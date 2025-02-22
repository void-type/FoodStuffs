using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ShoppingItemsWithAllRelatedSpecification : QuerySpecificationAbstract<ShoppingItem>
{
    public ShoppingItemsWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);

        AddCriteria(r => r.Id == id);
        AddInclude(nameof(ShoppingItem.Recipes));
        AddInclude(nameof(ShoppingItem.GroceryDepartment));
        AddInclude(nameof(ShoppingItem.PantryLocations));
    }
}
