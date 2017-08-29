namespace FoodStuffs.Model.Actions.Core.Responses.MessageString
{
    public abstract class MessageString
    {
        public string Message { get; set; }

        protected MessageString(string message = null)
        {
            Message = message;
        }
    }
}