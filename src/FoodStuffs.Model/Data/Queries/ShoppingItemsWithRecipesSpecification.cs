using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ShoppingItemsWithRecipesSpecification : QuerySpecificationAbstract<ShoppingItem>
{
    public ShoppingItemsWithRecipesSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(ShoppingItem.Recipes));
    }
}
