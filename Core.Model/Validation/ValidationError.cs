namespace Core.Model.Validation
{
    /// <summary>
    /// A UI-friendly error message with optional field name.
    /// </summary>
    public class ValidationError : IValidationError
    {
        /// <summary>
        /// UI friendly error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The entity property name that is in error. Can be mapped to a field on the view.
        /// </summary>
        public string FieldName { get; set; }

        public ValidationError(string errorMessage = null, string fieldName = null)
        {
            ErrorMessage = errorMessage;
            FieldName = fieldName;
        }
    }
}