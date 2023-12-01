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

        var pagedSearch = new RecipesSearchSpecification(
            criteria: searchCriteria.ToArray(),
            paginationOptions: paginationOptions,
            sortBy: request.SortBy);

        var recipes = await GetRecipes(request, paginationOptions, pagedSearch, cancellationToken);

        return recipes
            .Items
            .Select(r => new ListRecipesResponse(
                Id: r.Id,
                Name: r.Name,
                Categories: r.Categories.Select(c => c.Name).OrderBy(n => n),
                Ingredients: r.Ingredients
                    .Select(i => new ListRecipesResponseIngredient(i.Name, i.Quantity, i.Order, i.IsCategory))
                    .OrderBy(i => i.Order),
                Image: r.DefaultImage?.FileName))
            .ToItemSet(paginationOptions, recipes.TotalCount)
            .Map(Ok);
    }

    private async Task<IItemSet<Recipe>> GetRecipes(ListRecipesRequest request, PaginationOptions paginationOptions, RecipesSearchSpecification pagedSearch, CancellationToken cancellationToken)
    {
        if (string.Equals(request.SortBy, "RANDOM", StringComparison.OrdinalIgnoreCase))
        {
            // Due to a bug in EF with random sorts, we will get a list of IDs without using Includes.
            // We will also discard pagination until we can cache what items have been pulled.
            var randomIds = await _dbContext.Recipes
                .TagWith($"EF query called from: {nameof(ListRecipesHandler)}.{nameof(GetRecipes)}")
                .ApplyEfSpecification(pagedSearch, countAll: true)
                .Take(paginationOptions.Take)
                .OrderBy(_ => Guid.NewGuid())
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            // Get the hydrated items by ID.
            var randomIdSearch = new RecipesSearchSpecification(
                criteria: [x => randomIds.Contains(x.Id)],
                paginationOptions: PaginationOptions.None);

            var recipes = await _data.Recipes.ListPage(randomIdSearch, cancellationToken);

            // Always use page 1 of 1 for random sorts.
            var randomPaginationOptions = new PaginationOptions(1, paginationOptions.Take);

            // Order by our random IDs
            return recipes.Items
                .OrderBy(x => randomIds.IndexOf(x.Id))
                .ToItemSet(randomPaginationOptions, recipes.TotalCount);
        }

        return await _data.Recipes.ListPage(pagedSearch, cancellationToken);
    }

    private static List<Expression<Func<Recipe, bool>>> GetSearchCriteria(ListRecipesRequest request)
    {
        var searchCriteria = new List<Expression<Func<Recipe, bool>>>();

        // StringComparison overloads aren't supported in EF's SQL Server driver, but we want to ensure case-insensitive compare regardless of collation
        // Need to use Linq methods for EF
#pragma warning disable S6605, RCS1155, CA1862

        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            searchCriteria.Add(recipe => recipe.Name.ToLower().Contains(request.NameSearch.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(request.CategorySearch))
        {
            searchCriteria.Add(recipe => recipe.Categories.Any(c => c.Name.ToLower().Contains(request.CategorySearch.ToLower())));
        }

#pragma warning restore S6605, RCS1155, CA1862

        if (request.IsForMealPlanning is not null)
        {
            searchCriteria.Add(recipe => recipe.IsForMealPlanning == request.IsForMealPlanning);
        }

        return searchCriteria;
    }
}
