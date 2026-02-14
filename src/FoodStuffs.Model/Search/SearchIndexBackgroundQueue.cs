using System.Threading.Channels;

namespace FoodStuffs.Model.Search;

public class SearchIndexBackgroundQueue
{
    private readonly Channel<SearchIndexBackgroundAction> _channel = Channel.CreateUnbounded<SearchIndexBackgroundAction>(
        new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
        });

    public ChannelReader<SearchIndexBackgroundAction> Reader => _channel.Reader;

    public void Enqueue(SearchIndexBackgroundAction action)
    {
        if (action.EntityIds.Count == 0)
        {
            return;
        }

        _channel.Writer.TryWrite(action);
    }

    public void Clear(SearchIndex indexName)
    {
        var keep = new List<SearchIndexBackgroundAction>();

        while (_channel.Reader.TryRead(out var action))
        {
            if (action.IndexName != indexName)
            {
                keep.Add(action);
            }
        }

        foreach (var action in keep)
        {
            _channel.Writer.TryWrite(action);
        }
    }

    public void Complete()
    {
        _channel.Writer.Complete();
    }
}
