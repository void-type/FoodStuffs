using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

#pragma warning disable RCS1155

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
        var paginationOptions = new PaginationOptions(request.Page, request.Take, request.IsPagingEnabled);

        var searchCriteria = GetSearchCriteria(request);

        var allSearch = new RecipesSearchSpecification(searchCriteria);

        var totalCount = await _data.Recipes.Count(allSearch, cancellationToken).ConfigureAwait(false);

        var pagedSearch = new RecipesSearchSpecification(
            criteria: searchCriteria,
            paginationOptions: paginationOptions,
            sortBy: request.SortBy,
            sortDesc: request.SortDesc);

        var recipes = await _data.Recipes.List(pagedSearch, cancellationToken).ConfigureAwait(false);

        return recipes
            .Select(r => new ListRecipesResponse(
                Id: r.Id,
                Name: r.Name,
                Categories: r.CategoryRecipes.Select(cr => cr.Category.Name).OrderBy(n => n),
                ImageId: r.PinnedImageId ?? (r.Images.Count > 0 ? r.Images.Select(i => i.Id).FirstOrDefault() : (int?)null)))
            .ToItemSet(paginationOptions, totalCount)
            .Map(Ok);
    }

    private static Expression<Func<Recipe, bool>>[] GetSearchCriteria(ListRecipesRequest request)
    {
        var searchCriteria = new List<Expression<Func<Recipe, bool>>>();

        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            searchCriteria.Add(recipe => recipe.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(request.CategorySearch))
        {
            searchCriteria.Add(recipe => recipe.CategoryRecipes.Any(cr => cr.Category.Name.ToLower().Contains(request.CategorySearch.ToLower())));
        }

        return searchCriteria.ToArray();
    }
}
