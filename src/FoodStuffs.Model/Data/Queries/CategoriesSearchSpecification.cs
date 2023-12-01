using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Data.Queries;

public class CategoriesSearchSpecification : QuerySpecificationAbstract<Category>
{
    public CategoriesSearchSpecification(Expression<Func<Category, bool>>[] criteria, PaginationOptions paginationOptions) : base(criteria)
    {
        ApplyPaging(paginationOptions);
        AddOrderBy(m => m.Name);
    }
}
