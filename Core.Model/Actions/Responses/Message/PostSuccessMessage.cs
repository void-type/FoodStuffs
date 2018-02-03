namespace Core.Model.Actions.Responses.Message
{
    /// <summary>
    /// Store a UI-friendly success message and the Id of the entity that was altered.
    /// </summary>
    public class PostSuccessMessage : SuccessMessage
    {
        public string Id { get; set; }

        public PostSuccessMessage(string message = null, string id = null) : base(message)
        {
            Id = id;
        }
    }
}