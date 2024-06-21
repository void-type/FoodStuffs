namespace FoodStuffs.Model.Search;

public class SearchFacet
{
    public string FieldName { get; set; } = string.Empty;
    public List<SearchFacetValue> Values { get; init; } = [];
}
