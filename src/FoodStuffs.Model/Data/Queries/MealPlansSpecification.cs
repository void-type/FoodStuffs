using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealPlansSpecification : QuerySpecificationAbstract<MealPlan>
{
    public MealPlansSpecification()
    {
        AddOrderBy(m => m.CreatedOn, true);
    }
}
