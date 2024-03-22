using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class CategoriesSpecification : QuerySpecificationAbstract<Category>
{
    public CategoriesSpecification(Expression<Func<Category, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(c => c.Name);
    }
}
