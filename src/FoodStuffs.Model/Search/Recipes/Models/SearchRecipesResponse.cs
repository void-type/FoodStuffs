using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.Recipes.Models;

public record SearchRecipesResponse(
    IItemSet<SearchRecipesResultItem> Results,
    List<SearchFacet> Facets);
