namespace Core.Model.Actions.Responses.MessageString
{
    public class PostSuccessMessage : MessageString
    {
        public string Id { get; set; }

        public PostSuccessMessage(string message = null, string id = null) : base(message)
        {
            Id = id;
        }
    }
}