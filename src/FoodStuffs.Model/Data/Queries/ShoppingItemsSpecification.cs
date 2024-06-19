using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ShoppingItemsSpecification : QuerySpecificationAbstract<ShoppingItem>
{
    public ShoppingItemsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
    }

    public ShoppingItemsSpecification(IEnumerable<int> ids)
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
