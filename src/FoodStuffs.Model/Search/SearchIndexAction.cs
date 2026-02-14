namespace FoodStuffs.Model.Search;

public record SearchIndexBackgroundAction(SearchIndex IndexName, IReadOnlyList<int> EntityIds);
