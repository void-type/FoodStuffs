using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans;

public class ListMealPlansHandler : CustomEventHandlerAbstract<ListMealPlansRequest, IItemSet<ListMealPlansResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListMealPlansHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override Task<IResult<IItemSet<ListMealPlansResponse>>> Handle(ListMealPlansRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var searchCriteria = GetSearchCriteria(request);

        var specification = new MealPlansSpecification(criteria: searchCriteria);

        return _data.MealPlans
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(ms => new ListMealPlansResponse(
                Id: ms.Id,
                Name: ms.Name))
            .MapAsync(Ok);
    }

    private static Expression<Func<MealPlan, bool>>[] GetSearchCriteria(ListMealPlansRequest request)
    {
        var searchCriteria = new List<Expression<Func<MealPlan, bool>>>();

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
