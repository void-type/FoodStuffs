using Core.Model.Actions.Responder;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Response with an item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RespondWithItem<T> : AbstractActionStep
    {
        public RespondWithItem(T data, string logExtra = null)
        {
            _data = data;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithItem(_data, _logExtra);
        }

        private readonly T _data;
        private readonly string _logExtra;
    }
}