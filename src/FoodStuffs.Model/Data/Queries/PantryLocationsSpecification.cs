using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Events.PantryLocations.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class PantryLocationsSpecification : QuerySpecificationAbstract<PantryLocation>
{
    public PantryLocationsSpecification(Expression<Func<PantryLocation, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(c => c.Name);
    }

    public PantryLocationsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
    }

    public PantryLocationsSpecification(string name)
    {
        AddCriteria(r => r.Name == name);
    }

    public PantryLocationsSpecification(ListPantryLocationsRequest request) : this(criteria: [])
    {
        AddInclude(nameof(PantryLocation.GroceryItems));

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
            AddCriteria(m => m.GroceryItems.Any() != request.IsUnused);
        }

#pragma warning restore CA1860 // Avoid using 'Enumerable.Any()' extension method
    }
}
