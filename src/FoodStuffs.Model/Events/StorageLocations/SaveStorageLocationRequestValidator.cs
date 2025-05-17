using FoodStuffs.Model.Events.StorageLocations.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.StorageLocations;

public class SaveStorageLocationRequestValidator : RuleValidatorAbstract<SaveStorageLocationRequest>
{
    public SaveStorageLocationRequestValidator()
    {
        CreateRule(new Failure("Name is required.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Name can't be longer than 450 characters.", "name"))
            .InvalidWhen(entity => entity.Name?.Length > 450);
    }
}
