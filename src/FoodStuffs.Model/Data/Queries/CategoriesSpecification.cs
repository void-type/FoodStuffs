using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Events.Categories.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class CategoriesSpecification : QuerySpecificationAbstract<Category>
{
    public CategoriesSpecification(Expression<Func<Category, bool>>[] criteria) : this(criteria, applyDefaultOrdering: true)
    {
    }

    private CategoriesSpecification(Expression<Func<Category, bool>>[] criteria, bool applyDefaultOrdering) : base(criteria)
    {
        if (applyDefaultOrdering)
        {
            AddOrderBy(c => c.Name);
        }
    }

    public CategoriesSpecification(string name)
    {
        AddCriteria(r => r.Name == name);
    }

    public CategoriesSpecification(ListCategoriesRequest request) : this(criteria: [], applyDefaultOrdering: false)
    {
        AddInclude(nameof(Category.Recipes));
        AddOrderings(request);

        // StringComparison overloads aren't supported in EF's SQL Server driver, but we want to ensure case-insensitive compare regardless of collation
        // Need to use Linq methods for EF
#pragma warning disable S6605, RCS1155, CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparison

        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            AddCriteria(m => m.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

#pragma warning restore S6605, RCS1155, CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparison

        // We're using EF, so this is not a performance concern
#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

        if (request.IsUnused is not null)
        {
            AddCriteria(m => m.Recipes.Any() != request.IsUnused);
        }

#pragma warning restore CA1860 // Avoid using 'Enumerable.Any()' extension method
    }

    private void AddOrderings(ListCategoriesRequest request)
    {
        switch (request.SortBy?.ToUpperInvariant())
        {
            case "SHOWONCARDS":
                AddOrderBy(c => c.ShowOnCards, request.SortDescending);
                AddOrderBy(c => c.Name);
                break;
            case "RECIPECOUNT":
                AddOrderBy(c => c.Recipes.Count, request.SortDescending);
                AddOrderBy(c => c.Name);
                break;
            case "NAME":
                AddOrderBy(c => c.Name, request.SortDescending);
                break;
            default:
                AddOrderBy(c => c.Name);
                break;
        }
    }
}
