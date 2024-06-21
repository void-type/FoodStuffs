using Lucene.Net.Analysis.Standard;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace FoodStuffs.Model.Search;

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
        var taxonomyFolder = settings.GetTaxonomyFolder(indexName);

        _indexDir = FSDirectory.Open(indexFolder);
        var standardAnalyzer = new StandardAnalyzer(settings.LuceneVersion);

        var indexConfig = new IndexWriterConfig(settings.LuceneVersion, standardAnalyzer)
        {
            OpenMode = openMode
        };

        IndexWriter = new IndexWriter(_indexDir, indexConfig);

        _taxonomyDir = FSDirectory.Open(taxonomyFolder);
        TaxonomyWriter = new DirectoryTaxonomyWriter(_taxonomyDir, openMode);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _indexDir.Dispose();
                IndexWriter.Dispose();
                _taxonomyDir.Dispose();
                TaxonomyWriter.Dispose();
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
