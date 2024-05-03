using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealPlansSpecification : QuerySpecificationAbstract<MealPlan>
{
    public MealPlansSpecification(Expression<Func<MealPlan, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(m => m.CreatedOn, true);
    }
}
