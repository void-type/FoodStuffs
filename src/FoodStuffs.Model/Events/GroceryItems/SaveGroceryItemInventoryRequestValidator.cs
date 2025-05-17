using FoodStuffs.Model.Events.GroceryItems.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryItems;

public class SaveGroceryItemInventoryRequestValidator : RuleValidatorAbstract<SaveGroceryItemInventoryRequest>
{
    public SaveGroceryItemInventoryRequestValidator()
    {
        CreateRule(new Failure("Grocery item inventory quantity must be 0 or greater.", "inventoryQuantity"))
            .InvalidWhen(entity => entity.InventoryQuantity < 0);
    }
}
