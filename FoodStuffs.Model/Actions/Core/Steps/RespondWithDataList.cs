using FoodStuffs.Model.Actions.Core.Responder;
using System.Collections.Generic;

namespace FoodStuffs.Model.Actions.Core.Steps
{
    /// <summary>
    /// Response with Api data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RespondWithDataList<T> : ActionStep
    {
        public RespondWithDataList(IEnumerable<T> data, string logExtra = null)
        {
            _data = data;
            _logExtra = logExtra;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            respond.WithDataList(_data, _logExtra);
        }

        private readonly IEnumerable<T> _data;
        private readonly string _logExtra;
    }
}