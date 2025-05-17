using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class StorageLocationsWithAllRelatedSpecification : QuerySpecificationAbstract<StorageLocation>
{
    public StorageLocationsWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(StorageLocation.GroceryItems));
    }
}
