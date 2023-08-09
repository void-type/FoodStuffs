using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Data.Queries;

public class MealSetsSearchSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsSearchSpecification(Expression<Func<MealSet, bool>>[] criteria) : base(criteria)
    {
    }

    public MealSetsSearchSpecification(Expression<Func<MealSet, bool>>[] criteria, PaginationOptions paginationOptions) : this(criteria)
    {
        ApplyPaging(paginationOptions);
        AddOrderBy(m => m.CreatedOn, true);
    }
}
