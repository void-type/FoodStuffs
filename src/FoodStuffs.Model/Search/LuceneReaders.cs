using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace FoodStuffs.Model.Search;

public class LuceneReaders : IDisposable
{
    private bool _disposedValue;
    private readonly FSDirectory _indexDir;
    private readonly FSDirectory _taxonomyDir;

    public DirectoryReader IndexReader { get; }
    public IndexSearcher IndexSearcher { get; }
    public DirectoryTaxonomyReader TaxonomyReader { get; }

    public LuceneReaders(SearchSettings settings, string indexName)
    {
        var indexFolder = settings.GetIndexFolder(indexName);

        _indexDir = FSDirectory.Open(indexFolder);
        IndexReader = DirectoryReader.Open(_indexDir);
        IndexSearcher = new IndexSearcher(IndexReader);

        var taxonomyFolder = settings.GetTaxonomyFolder(indexName);

        _taxonomyDir = FSDirectory.Open(taxonomyFolder);
        TaxonomyReader = new DirectoryTaxonomyReader(_taxonomyDir);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                IndexReader.Dispose();
                _indexDir.Dispose();

                TaxonomyReader.Dispose();
                _taxonomyDir.Dispose();
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
