using DataMigratorV15.NewData.Models;

namespace DataMigratorV15;

internal class IngredientCategory
{
    public string Name { get; set; } = string.Empty;
    public List<RecipeIngredient> Items { get; set; } = [];
}
