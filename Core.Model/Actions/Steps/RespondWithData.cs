using Core.Model.Actions.Responder;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Response with Api data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RespondWithData<T> : AbstractActionStep
    {
        public RespondWithData(T data, string logExtra = null)
        {
            _data = data;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithData(_data, _logExtra);
        }

        private readonly T _data;
        private readonly string _logExtra;
    }
}