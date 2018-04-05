namespace Core.Model.Actions.Responses.ItemSet
{
    public interface IItemSetPage
    {
        int Page { get; set; }
        int TotalCount { get; set; }
    }
}