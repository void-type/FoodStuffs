using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class SaveShoppingItemHandler : CustomEventHandlerAbstract<SaveShoppingItemRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveShoppingItemRequestValidator _validator;
    private readonly IRecipeIndexService _index;

    public SaveShoppingItemHandler(FoodStuffsContext data, SaveShoppingItemRequestValidator validator, IRecipeIndexService index)
    {
        _data = data;
        _validator = validator;
        _index = index;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveShoppingItemRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveShoppingItemRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new ShoppingItemsSpecification(request.Id);

        var maybeShoppingItem = await _data.ShoppingItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .Include(si => si.Recipes)
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

            foreach (var id in shoppingItemToEdit.Recipes.ConvertAll(r => r.Id))
            {
                await _index.AddOrUpdateAsync(id, cancellationToken);
            }
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
