using Lucene.Net.Analysis.Standard;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using VoidCore.Model.Text;

namespace FoodStuffs.Model.Search;

public class LuceneWriters : IDisposable
{
    private bool _disposedValue;
    private readonly FSDirectory _indexDir;
    private readonly FSDirectory _taxonomyDir;

    public IndexWriter IndexWriter { get; }
    public DirectoryTaxonomyWriter TaxonomyWriter { get; }

    public LuceneWriters(RecipeSearchSettings settings, LuceneVersion version, OpenMode openMode)
    {
        var indexFolder = settings.IndexFolder.GetSafeFilePath();
        var taxonomyFolder = settings.TaxonomyFolder.GetSafeFilePath();

        _indexDir = FSDirectory.Open(indexFolder);
        var standardAnalyzer = new StandardAnalyzer(version);

        var indexConfig = new IndexWriterConfig(version, standardAnalyzer)
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
