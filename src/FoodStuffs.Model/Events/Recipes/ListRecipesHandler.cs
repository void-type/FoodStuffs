using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VoidCore.EntityFramework;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Recipes;

public class ListRecipesHandler : EventHandlerAbstract<ListRecipesRequest, IItemSet<ListRecipesResponse>>
{
    private readonly IFoodStuffsData _data;
    private readonly FoodStuffsContext _dbContext;

    public ListRecipesHandler(IFoodStuffsData data, FoodStuffsContext dbContext)
    {
        _data = data;
        _dbContext = dbContext;
    }

    public override async Task<IResult<IItemSet<ListRecipesResponse>>> Handle(ListRecipesRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var searchCriteria = GetSearchCriteria(request);

        var allSearch = new RecipesSearchSpecification(searchCriteria.ToArray());

        var totalCount = await _data.Recipes.Count(allSearch, cancellationToken);

        var recipes = await GetRecipes(request, paginationOptions, searchCriteria, allSearch, cancellationToken);

        return recipes
            .Select(r => new ListRecipesResponse(
                Id: r.Id,
                Name: r.Name,
                Categories: r.Categories.Select(c => c.Name).OrderBy(n => n),
                Ingredients: r.Ingredients
                    .Select(i => new ListRecipesResponseIngredient(i.Name, i.Quantity, i.Order, i.IsCategory))
                    .OrderBy(i => i.Order),
                Image: r.DefaultImage?.FileName))
            .ToItemSet(paginationOptions, totalCount)
            .Map(Ok);
    }

    private async Task<IReadOnlyList<Recipe>> GetRecipes(ListRecipesRequest request, PaginationOptions paginationOptions, List<Expression<Func<Recipe, bool>>> searchCriteria, RecipesSearchSpecification allSearch, CancellationToken cancellationToken)
    {
        if (string.Equals(request.SortBy, "RANDOM", StringComparison.OrdinalIgnoreCase))
        {
            // Due to a bug in EF with random sorts, we will get a list of IDs without using Includes.
            var randomIds = await _dbContext.Recipes
                .TagWith($"EF query called from: {nameof(ListRecipesHandler)}.{nameof(GetRecipes)}")
                .ApplyEfSpecification(allSearch)
                .GetPage(paginationOptions)
                .OrderBy(_ => Guid.NewGuid())
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            // Get the hydrated items by ID.
            var randomPagedSearch = new RecipesSearchSpecification(
                criteria: new Expression<Func<Recipe, bool>>[] { x => randomIds.Contains(x.Id) },
                paginationOptions: PaginationOptions.None);

            // Order by our random IDs
            return (await _data.Recipes.List(randomPagedSearch, cancellationToken))
                .OrderBy(x => randomIds.IndexOf(x.Id))
                .ToList();
        }

        var pagedSearch = new RecipesSearchSpecification(
            criteria: searchCriteria.ToArray(),
            paginationOptions: paginationOptions,
            sortBy: request.SortBy);

        return await _data.Recipes.List(pagedSearch, cancellationToken);
    }

    private static List<Expression<Func<Recipe, bool>>> GetSearchCriteria(ListRecipesRequest request)
    {
        var searchCriteria = new List<Expression<Func<Recipe, bool>>>();

        // StringComparison overloads aren't supported in EF's SQL Server driver, but we want to ensure case-insensitive compare regardless of collation
#pragma warning disable RCS1155
        // Need to use Linq methods for EF
#pragma warning disable S6605

        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            searchCriteria.Add(recipe => recipe.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(request.CategorySearch))
        {
            searchCriteria.Add(recipe => recipe.Categories.Any(c => c.Name.ToLower().Contains(request.CategorySearch.ToLower())));
        }

#pragma warning restore S6605
#pragma warning restore RCS1155

        if (request.IsForMealPlanning is not null)
        {
            searchCriteria.Add(recipe => recipe.IsForMealPlanning == request.IsForMealPlanning);
        }

        return searchCriteria;
    }
}
