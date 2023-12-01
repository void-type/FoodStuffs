using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using System.Linq.Expressions;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Categories;

public class ListCategoriesHandler : EventHandlerAbstract<ListCategoriesRequest, IItemSet<ListCategoriesResponse>>
{
    private readonly IFoodStuffsData _data;

    public ListCategoriesHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListCategoriesResponse>>> Handle(ListCategoriesRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var searchCriteria = GetSearchCriteria(request);

        var pagedSearch = new CategoriesSearchSpecification(
            criteria: searchCriteria,
            paginationOptions: paginationOptions);

        var categories = await _data.Categories.ListPage(pagedSearch, cancellationToken);

        return categories
            .Items
            .Select(ms => new ListCategoriesResponse(
                Id: ms.Id,
                Name: ms.Name))
            .ToItemSet(paginationOptions, categories.TotalCount)
            .Map(Ok);
    }

    private static Expression<Func<Category, bool>>[] GetSearchCriteria(ListCategoriesRequest request)
    {
        var searchCriteria = new List<Expression<Func<Category, bool>>>();

        // StringComparison overloads aren't supported in EF's SQL Server driver, but we want to ensure case-insensitive compare regardless of collation
        // Need to use Linq methods for EF
#pragma warning disable S6605, RCS1155, CA1862

        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            searchCriteria.Add(m => m.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

#pragma warning restore S6605, RCS1155, CA1862

        return searchCriteria.ToArray();
    }
}
