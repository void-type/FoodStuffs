namespace FoodStuffs.Model.Search;

public interface IRecipeQueryService
{
    RecipeSearchResponse Search(RecipeSearchRequest request);
}
