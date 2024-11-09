using DataMigratorV14.OldData.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace DataMigratorV14.OldData.Queries;

public class MealSetsSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsSpecification(Expression<Func<MealSet, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(m => m.CreatedOn, true);
    }
}
