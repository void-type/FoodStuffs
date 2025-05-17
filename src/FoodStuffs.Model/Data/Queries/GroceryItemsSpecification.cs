using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Events.GroceryItems.Models;
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

    public GroceryItemsSpecification(ListGroceryItemsRequest request) : this(criteria: [])
    {
        AddInclude(nameof(GroceryItem.StorageLocations));
        AddInclude(nameof(GroceryItem.Recipes));

        // StringComparison overloads aren't supported in EF's SQL Server driver, but we want to ensure case-insensitive compare regardless of collation
        // Need to use Linq methods for EF
#pragma warning disable S6605, RCS1155, CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparison

        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            AddCriteria(m => m.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

#pragma warning restore S6605, RCS1155, CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparison

        // We're using EF, so this is not a performance concern
#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

        if (request.IsUnused is not null)
        {
            AddCriteria(m => m.Recipes.Any() != request.IsUnused);
        }

#pragma warning restore CA1860 // Avoid using 'Enumerable.Any()' extension method
    }
}
