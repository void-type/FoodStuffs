namespace Core.Model.Actions.Responses.ItemSet
{
    public class PagedItemSet<TEntity> : CountedItemSet<TEntity>, IPagedItemSet
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
    }
}