using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ShoppingItemsWithRecipesAndGroceryDepartmentsSpecification : QuerySpecificationAbstract<ShoppingItem>
{
    public ShoppingItemsWithRecipesAndGroceryDepartmentsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(ShoppingItem.Recipes));
        AddInclude(nameof(ShoppingItem.GroceryDepartment));
    }
}
