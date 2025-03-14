using FoodStuffs.Model.Events.ShoppingItems.Models;
using VoidCore.Model.Functional;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemInventoryRequestValidator : RuleValidatorAbstract<SaveShoppingItemInventoryRequest>
{
    public SaveShoppingItemInventoryRequestValidator()
    {
        CreateRule(new Failure("Grocery item inventory quantity must be 0 or greater.", "inventoryQuantity"))
            .InvalidWhen(entity => entity.InventoryQuantity < 0);
    }
}
