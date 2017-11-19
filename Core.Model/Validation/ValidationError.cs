namespace Core.Model.Validation
{
    /// <summary>
    /// A dto for sending form validation errors to the UI. This can be used in a M:N configuration where the error message and the field name can be
    /// reused in the error set.
    /// </summary>
    public class ValidationError : IValidationError
    {
        /// <summary>
        /// UI friendly error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The entity property name that is in error.
        /// </summary>
        public string FieldName { get; set; }

        public ValidationError(string errorMessage = null, string fieldName = null)
        {
            ErrorMessage = errorMessage;
            FieldName = fieldName;
        }
    }
}