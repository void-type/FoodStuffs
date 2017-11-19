using Core.Model.Actions.Responses.MessageString;
using Core.Model.Validation;
using System.Collections.Generic;

namespace FoodStuffs.Test.Mocks.Action
{
    public class SimpleResponse
    {
        public object DataItem = null;
        public List<object> DataList = new List<object>();
        public ErrorMessage Error = null;
        public SuccessMessage Success = null;
        public List<IValidationError> ValidationErrors = new List<IValidationError>();
    }
}