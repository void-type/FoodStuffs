using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ShoppingItemsSpecification : QuerySpecificationAbstract<ShoppingItem>
{
    public ShoppingItemsSpecification(Expression<Func<ShoppingItem, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(c => c.Name);
    }

    public ShoppingItemsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
    }

    public ShoppingItemsSpecification(string name)
    {
        AddCriteria(r => r.Name == name);
    }

    public ShoppingItemsSpecification(IEnumerable<int> ids)
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
