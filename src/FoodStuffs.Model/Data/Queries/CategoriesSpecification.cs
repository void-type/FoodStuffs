using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Data.Queries;

public class CategoriesSpecification : QuerySpecificationAbstract<Category>
{
    public CategoriesSpecification(Expression<Func<Category, bool>>[] criteria, PaginationOptions paginationOptions) : base(criteria)
    {
        ApplyPaging(paginationOptions);
        AddOrderBy(m => m.Name);
    }
}
