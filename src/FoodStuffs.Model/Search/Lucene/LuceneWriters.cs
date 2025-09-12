using Lucene.Net.Analysis.Standard;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace FoodStuffs.Model.Search.Lucene;

public class LuceneWriters : IDisposable
{
    private bool _disposedValue;
    private readonly FSDirectory _indexDir;
    private readonly FSDirectory _taxonomyDir;

    public IndexWriter IndexWriter { get; }
    public DirectoryTaxonomyWriter TaxonomyWriter { get; }

    public LuceneWriters(SearchSettings settings, OpenMode openMode, string indexName)
    {
        var indexFolder = settings.GetIndexFolder(indexName);

        _indexDir = FSDirectory.Open(indexFolder);
        var standardAnalyzer = new StandardAnalyzer(settings.LuceneVersion);

        var indexConfig = new IndexWriterConfig(settings.LuceneVersion, standardAnalyzer)
        {
            OpenMode = openMode
        };

        IndexWriter = new IndexWriter(_indexDir, indexConfig);

        var taxonomyFolder = settings.GetTaxonomyFolder(indexName);

        _taxonomyDir = FSDirectory.Open(taxonomyFolder);
        TaxonomyWriter = new DirectoryTaxonomyWriter(_taxonomyDir, openMode);

        if (openMode != OpenMode.CREATE)
        {
            // Ensure index
            IndexWriter.Commit();
            TaxonomyWriter.Commit();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                IndexWriter.Dispose();
                _indexDir.Dispose();

                TaxonomyWriter.Dispose();
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
