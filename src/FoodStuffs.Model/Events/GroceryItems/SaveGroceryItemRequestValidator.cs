using FoodStuffs.Model.Events.GroceryItems.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryItems;

public class SaveGroceryItemRequestValidator : RuleValidatorAbstract<SaveGroceryItemRequest>
{
    public SaveGroceryItemRequestValidator()
    {
        CreateRule(new Failure("Name is required.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Name can't be longer than 450 characters.", "name"))
            .InvalidWhen(entity => entity.Name.Length > 450);

        CreateRule(new Failure("Inventory quantity must be 0 or greater.", "inventoryQuantity"))
            .InvalidWhen(entity => entity.InventoryQuantity < 0);

        CreateRule(new Failure("Storage location names can't be longer than 450 characters.", "storageLocations"))
            .InvalidWhen(entity => entity.StorageLocations?.Exists(x => x.Length > 450) ?? false);
    }
}
