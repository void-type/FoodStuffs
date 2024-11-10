using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class ListShoppingItemsHandler : CustomEventHandlerAbstract<ListShoppingItemsRequest, IItemSet<ListShoppingItemsResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListShoppingItemsHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListShoppingItemsResponse>>> Handle(ListShoppingItemsRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var searchCriteria = GetSearchCriteria(request);

        var specification = new ShoppingItemsSpecification(criteria: searchCriteria);

        return await _data.ShoppingItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(c => new ListShoppingItemsResponse(
                Id: c.Id,
                Name: c.Name))
            .MapAsync(Ok);
    }

    private static Expression<Func<ShoppingItem, bool>>[] GetSearchCriteria(ListShoppingItemsRequest request)
    {
        var searchCriteria = new List<Expression<Func<ShoppingItem, bool>>>();

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
