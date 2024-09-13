using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemHandler : CustomEventHandlerAbstract<SaveShoppingItemRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public SaveShoppingItemHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveShoppingItemRequest request, CancellationToken cancellationToken = default)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new ShoppingItemsSpecification(request.Id);

        var maybeShoppingItem = await _data.ShoppingItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new ShoppingItemsSpecification(requestedName);

        var conflictingShoppingItem = await _data.ShoppingItems
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingShoppingItem is not null && conflictingShoppingItem.Id != request.Id)
        {
            return Fail(new Failure("Shopping item name already exists.", "shoppingItemName"));
        }

        var shoppingItemToEdit = maybeShoppingItem.Unwrap(() => new ShoppingItem());

        Transfer(requestedName, shoppingItemToEdit);

        if (maybeShoppingItem.HasValue)
        {
            _data.ShoppingItems.Update(shoppingItemToEdit);
        }
        else
        {
            await _data.ShoppingItems.AddAsync(shoppingItemToEdit, cancellationToken);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Shopping item {(maybeShoppingItem.HasValue ? "updated" : "added")}.", shoppingItemToEdit.Id));
    }

    private static void Transfer(string formattedName, ShoppingItem shoppingItem)
    {
        shoppingItem.Name = formattedName;
    }
}
