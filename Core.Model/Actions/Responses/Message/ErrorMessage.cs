namespace Core.Model.Actions.Responses.Message
{
    /// <summary>
    /// Stores a UI-friendly error message for critical or non-user caused errors.
    /// </summary>
    public class ErrorMessage : IMessage
    {
        public string Message { get; set; }

        public ErrorMessage(string message = null)
        {
            Message = message;
        }
    }
}