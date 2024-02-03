using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Data.Queries;

public class MealSetsSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsSpecification(Expression<Func<MealSet, bool>>[] criteria, PaginationOptions paginationOptions) : base(criteria)
    {
        ApplyPaging(paginationOptions);
        AddOrderBy(m => m.CreatedOn, true);
    }
}
