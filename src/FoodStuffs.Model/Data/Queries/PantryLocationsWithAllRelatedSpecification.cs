using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class PantryLocationsWithAllRelatedSpecification : QuerySpecificationAbstract<PantryLocation>
{
    public PantryLocationsWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(PantryLocation.ShoppingItems));
    }
}
