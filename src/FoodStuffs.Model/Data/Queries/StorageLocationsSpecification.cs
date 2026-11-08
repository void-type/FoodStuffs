using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Events.StorageLocations.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class StorageLocationsSpecification : QuerySpecificationAbstract<StorageLocation>
{
    public StorageLocationsSpecification(Expression<Func<StorageLocation, bool>>[] criteria) : this(criteria, applyDefaultOrdering: true)
    {
    }

    private StorageLocationsSpecification(Expression<Func<StorageLocation, bool>>[] criteria, bool applyDefaultOrdering) : base(criteria)
    {
        if (applyDefaultOrdering)
        {
            AddOrderBy(c => c.Name);
        }
    }

    public StorageLocationsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
    }

    public StorageLocationsSpecification(string name)
    {
        AddCriteria(r => r.Name == name);
    }

    public StorageLocationsSpecification(ListStorageLocationsRequest request) : this(criteria: [], applyDefaultOrdering: false)
    {
        AddInclude(nameof(StorageLocation.GroceryItems));
        AddOrderings(request);

        // StringComparison overloads aren't supported in EF's SQL Server driver, but we want to ensure case-insensitive compare regardless of collation
        // Need to use Linq methods for EF
        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            AddCriteria(m => m.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

        if (request.IsUnused is not null)
        {
            AddCriteria(m => m.GroceryItems.Any() != request.IsUnused);
        }
    }

    private void AddOrderings(ListStorageLocationsRequest request)
    {
        switch (request.SortBy?.ToUpperInvariant())
        {
            case "GROCERYITEMCOUNT":
                AddOrderBy(c => c.GroceryItems.Count, request.SortDescending);
                AddOrderBy(c => c.Name);
                break;
            case "NAME":
                AddOrderBy(c => c.Name, request.SortDescending);
                break;
            default:
                AddOrderBy(c => c.Name);
                break;
        }
    }
}
