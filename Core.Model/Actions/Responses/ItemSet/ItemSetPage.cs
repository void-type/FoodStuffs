namespace Core.Model.Actions.Responses.ItemSet
{
    public class ItemSetPage<TEntity> : CountedItemSet<TEntity>, IItemSetPage
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public int TotalCount { get; set; }
    }
}