using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search;

public record RecipeSearchResponse(
    IItemSet<RecipeSearchResultItem> Results,
    IEnumerable<RecipeSearchFacet> Facets);
