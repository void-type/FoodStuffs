namespace FoodStuffs.Model.Validation.Core
{
    public interface IValidationError
    {
        string ErrorMessage { get; set; }
        string FieldName { get; set; }
    }
}