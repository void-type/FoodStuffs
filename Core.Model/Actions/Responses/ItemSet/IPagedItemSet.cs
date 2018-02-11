namespace Core.Model.Actions.Responses.ItemSet
{
    public interface IPagedItemSet
    {
        int Page { get; set; }
        int TotalCount { get; set; }
    }
}