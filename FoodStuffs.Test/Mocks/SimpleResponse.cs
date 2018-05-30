using Core.Model.Actions.Responses.File;
using Core.Model.Actions.Responses.Message;
using Core.Model.Validation;
using System.Collections.Generic;

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