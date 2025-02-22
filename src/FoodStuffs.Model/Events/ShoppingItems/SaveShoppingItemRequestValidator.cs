using FoodStuffs.Model.Events.ShoppingItems.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemRequestValidator : RuleValidatorAbstract<SaveShoppingItemRequest>
{
    public SaveShoppingItemRequestValidator()
    {
        CreateRule(new Failure("Name is required.", "name"))
            .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

        CreateRule(new Failure("Name can't be longer than 450 characters.", "name"))
            .InvalidWhen(entity => entity.Name.Length > 450);

        CreateRule(new Failure("Inventory quantity must be 0 or greater.", "inventoryQuantity"))
            .InvalidWhen(entity => entity.InventoryQuantity < 0);

        CreateRule(new Failure("Pantry location names can't be longer than 450 characters.", "pantryLocations"))
            .InvalidWhen(entity => entity.PantryLocations.Exists(x => x.Length > 450));
    }
}
