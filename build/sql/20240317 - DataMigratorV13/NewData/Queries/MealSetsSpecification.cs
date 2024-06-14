using DataMigratorV13.NewData.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace DataMigratorV13.NewData.Queries;

public class MealSetsSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsSpecification(Expression<Func<MealSet, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(m => m.CreatedOn, true);
    }
}
