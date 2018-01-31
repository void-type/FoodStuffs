using Core.Model.Actions.Responder;
using System.Collections.Generic;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Response with a collection of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RespondWithList<T> : AbstractActionStep
    {
        public RespondWithList(IEnumerable<T> data, string logExtra = null)
        {
            _data = data;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithList(_data, _logExtra);
        }

        private readonly IEnumerable<T> _data;
        private readonly string _logExtra;
    }
}