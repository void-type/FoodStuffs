using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using System.Linq.Expressions;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class ListRecipesHandler : EventHandlerAbstract<ListRecipesRequest, IItemSet<ListRecipesResponse>>
{
    private readonly IFoodStuffsData _data;

    public ListRecipesHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListRecipesResponse>>> Handle(ListRecipesRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var searchCriteria = GetSearchCriteria(request);

        var allSearch = new RecipesSearchSpecification(searchCriteria);

        var totalCount = await _data.Recipes.Count(allSearch, cancellationToken);

        var pagedSearch = new RecipesSearchSpecification(
            criteria: searchCriteria,
            paginationOptions: paginationOptions,
            sortBy: request.SortBy,
            sortDesc: request.SortDesc);

        var recipes = await _data.Recipes.List(pagedSearch, cancellationToken);

        return recipes
            .Select(r => new ListRecipesResponse(
                Id: r.Id,
                Name: r.Name,
                Categories: r.Categories.Select(c => c.Name).OrderBy(n => n),
                ImageId: r.PinnedImageId ?? r.Images.FirstOrDefault()?.Id))
            .ToItemSet(paginationOptions, totalCount)
            .Map(Ok);
    }

    private static Expression<Func<Recipe, bool>>[] GetSearchCriteria(ListRecipesRequest request)
    {
        var searchCriteria = new List<Expression<Func<Recipe, bool>>>();

        // StringComparison overloads aren't supported in EF's SQL Server driver, but we want to ensure case-insensitive compare regardless of collation
#pragma warning disable RCS1155

        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            searchCriteria.Add(recipe => recipe.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(request.CategorySearch))
        {
            searchCriteria.Add(recipe => recipe.Categories.Any(c => c.Name.ToLower().Contains(request.CategorySearch.ToLower())));
        }

#pragma warning restore RCS1155

        if (request.IsForMealPlanning is not null)
        {
            searchCriteria.Add(recipe => recipe.IsForMealPlanning == request.IsForMealPlanning);
        }

        return searchCriteria.ToArray();
    }
}
