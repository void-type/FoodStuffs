namespace FoodStuffs.Model.Search;

public class RecipeSearchFacet
{
    public string FieldName { get; set; } = string.Empty;
    public List<RecipeSearchFacetValue> Values { get; init; } = [];
}
