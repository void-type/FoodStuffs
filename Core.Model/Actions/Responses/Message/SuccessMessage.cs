namespace Core.Model.Actions.Responses.Message
{
    /// <summary>
    /// Stores a UI-friendly success message.
    /// </summary>
    public class SuccessMessage : IMessage
    {
        public string Message { get; set; }

        public SuccessMessage(string message = null)
        {
            Message = message;
        }
    }
}