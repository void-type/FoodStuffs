
using VoidCore.Model.Guards;
using VoidCore.Model.Text;

namespace FoodStuffs.Model.Search;

public class RecipeSearchSettings
{
    public string IndexFolder { get; init; } = "App_Data/Lucene/RecipeIndex";
    public string TaxonomyFolder { get; init; } = "App_Data/Lucene/RecipeTaxonomy";

    public void Validate()
    {
        IndexFolder.EnsureNotNullOrEmpty();

        if (IndexFolder != IndexFolder.GetSafeFilePath())
        {
            throw new InvalidOperationException($"{nameof(IndexFolder)} is not a safe file path. Value: {IndexFolder}");
        }

        TaxonomyFolder.EnsureNotNullOrEmpty();

        if (TaxonomyFolder != TaxonomyFolder.GetSafeFilePath())
        {
            throw new InvalidOperationException($"{nameof(TaxonomyFolder)} is not a safe file path. Value: {TaxonomyFolder}");
        }
    }
}
