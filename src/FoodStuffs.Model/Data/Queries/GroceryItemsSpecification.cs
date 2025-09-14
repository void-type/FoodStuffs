using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class GroceryItemsSpecification : QuerySpecificationAbstract<GroceryItem>
{
    public GroceryItemsSpecification(Expression<Func<GroceryItem, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(c => c.Name);
    }

    public GroceryItemsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
    }

    public GroceryItemsSpecification(string name)
    {
        AddCriteria(r => r.Name == name);
    }

    public GroceryItemsSpecification(IEnumerable<int> ids) : this(criteria: [])
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
