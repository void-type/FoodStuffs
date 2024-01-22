using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using VoidCore.Model.Text;

namespace FoodStuffs.Model.Search;

public class LuceneReaders : IDisposable
{
    private bool _disposedValue;
    private readonly FSDirectory _indexDir;
    private readonly FSDirectory _taxonomyDir;

    public DirectoryReader IndexReader { get; }
    public IndexSearcher IndexSearcher { get; }
    public DirectoryTaxonomyReader TaxonomyReader { get; }

    public LuceneReaders(RecipeSearchSettings settings)
    {
        var indexFolder = settings.IndexFolder.GetSafeFilePath();
        var taxonomyFolder = settings.TaxonomyFolder.GetSafeFilePath();

        _indexDir = FSDirectory.Open(indexFolder);
        IndexReader = DirectoryReader.Open(_indexDir);
        IndexSearcher = new IndexSearcher(IndexReader);

        _taxonomyDir = FSDirectory.Open(taxonomyFolder);
        TaxonomyReader = new DirectoryTaxonomyReader(_taxonomyDir);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _indexDir.Dispose();
                IndexReader.Dispose();
                _taxonomyDir.Dispose();
                TaxonomyReader.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
