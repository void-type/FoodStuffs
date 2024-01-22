namespace FoodStuffs.Model.Search;

public interface IRecipeIndexService
{
    Task AddOrUpdate(int recipeId, CancellationToken cancellationToken);
    Task Rebuild();
    void Remove(int recipeId);
}
