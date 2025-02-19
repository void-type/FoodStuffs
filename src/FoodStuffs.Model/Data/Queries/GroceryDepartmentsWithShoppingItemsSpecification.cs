using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class GroceryDepartmentsWithShoppingItemsSpecification : QuerySpecificationAbstract<GroceryDepartment>
{
    public GroceryDepartmentsWithShoppingItemsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(GroceryDepartment.ShoppingItems));
    }
}
