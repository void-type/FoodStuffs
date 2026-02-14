using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryAisles.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryAisles;

public class SaveGroceryAisleHandler : CustomEventHandlerAbstract<SaveGroceryAisleRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveGroceryAisleRequestValidator _validator;
    private readonly ISearchIndexService _searchIndex;

    public SaveGroceryAisleHandler(FoodStuffsContext data, SaveGroceryAisleRequestValidator validator, ISearchIndexService searchIndex)
    {
        _data = data;
        _validator = validator;
        _searchIndex = searchIndex;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveGroceryAisleRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveGroceryAisleRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new GroceryAislesSpecification(request.Id);

        var maybeGroceryAisle = await _data.GroceryAisles
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .Include(x => x.GroceryItems)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new GroceryAislesSpecification(requestedName);

        var conflictingGroceryAisle = await _data.GroceryAisles
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingGroceryAisle is not null && conflictingGroceryAisle.Id != request.Id)
        {
            return Fail(new Failure("Grocery aisle name already exists.", "name"));
        }

        var groceryAisleToEdit = maybeGroceryAisle.Unwrap(() => new GroceryAisle());
        var relatedGroceryItemIds = groceryAisleToEdit.GroceryItems.Select(r => r.Id).ToHashSet();

        Transfer(request, requestedName, groceryAisleToEdit);

        if (maybeGroceryAisle.HasValue)
        {
            _data.GroceryAisles.Update(groceryAisleToEdit);
        }
        else
        {
            _data.GroceryAisles.Add(groceryAisleToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        relatedGroceryItemIds.UnionWith(groceryAisleToEdit.GroceryItems.Select(gi => gi.Id));

        _searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, relatedGroceryItemIds);

        return Ok(EntityMessage.Create($"Grocery Aisle {(maybeGroceryAisle.HasValue ? "updated" : "added")}.", groceryAisleToEdit.Id));
    }

    private static void Transfer(SaveGroceryAisleRequest request, string formattedName, GroceryAisle groceryAisle)
    {
        groceryAisle.Name = formattedName;
        groceryAisle.Order = request.Order;
    }
}
