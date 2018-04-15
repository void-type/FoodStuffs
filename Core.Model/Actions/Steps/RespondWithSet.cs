using Core.Model.Actions.Responder;
using System.Collections.Generic;

namespace Core.Model.Actions.Steps
{
    /// <summary>
    /// Response with a collection of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RespondWithSet<T> : AbstractActionStep
    {
        public RespondWithSet(IEnumerable<T> set, string logExtra = null)
        {
            _set = set;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithSet(_set, _logExtra);
        }

        private readonly IEnumerable<T> _set;
        private readonly string _logExtra;
    }
}