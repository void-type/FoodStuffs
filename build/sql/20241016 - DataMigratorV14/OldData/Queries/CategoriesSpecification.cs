using DataMigratorV14.OldData.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace DataMigratorV14.OldData.Queries;

public class CategoriesSpecification : QuerySpecificationAbstract<Category>
{
    public CategoriesSpecification(Expression<Func<Category, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(c => c.Name);
    }
}
