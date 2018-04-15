using Core.Model.Actions.Responses.Message;
using Core.Model.Validation;
using System.Collections.Generic;

namespace Core.Model.Actions.Responses
{
    /// <summary>
    /// Used with the SimpleActionResponder to capture action outputs in testing.
    /// </summary>
    public class SimpleResponse
    {
        public readonly List<IValidationError> ValidationErrors = new List<IValidationError>();
        public object Item = null;
        public List<object> Set = null;
        public ErrorMessage Error = null;
        public PostSuccessMessage PostSuccess = null;
        public SuccessMessage Success = null;
    }
}