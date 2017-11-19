namespace Core.Model.Validation
{
    public interface IValidationError
    {
        string ErrorMessage { get; set; }
        string FieldName { get; set; }
    }
}