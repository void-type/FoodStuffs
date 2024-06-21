
using Lucene.Net.Util;
using VoidCore.Model.Guards;
using VoidCore.Model.Text;

namespace FoodStuffs.Model.Search;

public class SearchSettings
{
    public string IndexFolder { get; init; } = "App_Data/Lucene";
    public LuceneVersion LuceneVersion { get; } = LuceneVersion.LUCENE_48;
    public int MaxResults { get; } = 1000;

    public string GetIndexFolder(string indexName)
    {
        return ValidateFolder($"{IndexFolder}/{indexName}Index");
    }

    public string GetTaxonomyFolder(string indexName)
    {
        return ValidateFolder($"{IndexFolder}/{indexName}Taxonomy");
    }

    public void Validate()
    {
        ValidateFolder(IndexFolder);
    }

    private string ValidateFolder(string folder)
    {
        folder.EnsureNotNullOrEmpty();

        if (folder != folder.GetSafeFilePath())
        {
            throw new InvalidOperationException($"Folder is not a safe file path. Value: {folder}");
        }

        return folder;
    }
}
