namespace FoodStuffs.Model.Search.Lucene;

public static class LuceneDirectoryHelper
{
    /// <summary>
    /// Cleans up old renamed index directories then renames the current index and taxonomy
    /// directories to free their paths for a fresh rebuild. Renaming succeeds on Windows
    /// even with open memory-mapped file handles. Old directories are cleaned up on each
    /// call; errors are ignored since handles may still be open.
    /// </summary>
    public static void PrepareForRebuild(SearchSettings settings, string indexName)
    {
        var indexFolder = settings.GetIndexFolder(indexName);
        var taxonomyFolder = settings.GetTaxonomyFolder(indexName);

        CleanupOldDirectories(indexFolder);
        CleanupOldDirectories(taxonomyFolder);

        RenameToOld(indexFolder);
        RenameToOld(taxonomyFolder);
    }

    private static void CleanupOldDirectories(string folder)
    {
        var parent = Path.GetDirectoryName(folder);
        var name = Path.GetFileName(folder);

        if (parent is null || name is null || !Directory.Exists(parent))
        {
            return;
        }

        foreach (var dir in Directory.GetDirectories(parent, $"{name}_old_*"))
        {
            try
            {
                Directory.Delete(dir, recursive: true);
            }
            catch
            {
                // Handles may still be open; will be retried on the next rebuild.
            }
        }
    }

    private static void RenameToOld(string folder)
    {
        if (!Directory.Exists(folder))
        {
            return;
        }

        var i = 1;
        string dest;

        do
        {
            dest = $"{folder}_old_{i}";
            i++;
        }
        while (Directory.Exists(dest));

        Directory.Move(folder, dest);
    }
}
