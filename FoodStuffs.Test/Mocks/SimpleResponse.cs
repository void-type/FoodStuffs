using System.Collections.Generic;
using VoidCore.Model.Actions.Responses.File;
using VoidCore.Model.Actions.Responses.Message;
using VoidCore.Model.Validation;

namespace FoodStuffs.Test.Mocks
{
    /// <summary>
    /// Used with the SimpleActionResponder to capture action outputs in testing.
    /// </summary>
    public class SimpleResponse
    {
        public readonly List<IValidationError> ValidationErrors = new List<IValidationError>();
        public ErrorMessage Error = null;
        public object Item = null;
        public PostSuccessMessage PostSuccess = null;
        public List<object> Set = null;
        public SuccessMessage Success = null;
        public IFileViewModel File { get; set; }
    }
}
