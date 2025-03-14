using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.PantryLocations.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.PantryLocations;

public class SavePantryLocationHandler : CustomEventHandlerAbstract<SavePantryLocationRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SavePantryLocationRequestValidator _validator;

    public SavePantryLocationHandler(FoodStuffsContext data, SavePantryLocationRequestValidator validator)
    {
        _data = data;
        _validator = validator;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SavePantryLocationRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SavePantryLocationRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new PantryLocationsWithAllRelatedSpecification(request.Id);

        var maybePantryLocation = await _data.PantryLocations
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new PantryLocationsSpecification(requestedName);

        var conflictingPantryLocation = await _data.PantryLocations
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingPantryLocation is not null && conflictingPantryLocation.Id != request.Id)
        {
            return Fail(new Failure("Storage location name already exists.", "name"));
        }

        var pantryLocationToEdit = maybePantryLocation.Unwrap(() => new PantryLocation());

        Transfer(requestedName, pantryLocationToEdit);

        if (maybePantryLocation.HasValue)
        {
            _data.PantryLocations.Update(pantryLocationToEdit);
        }
        else
        {
            _data.PantryLocations.Add(pantryLocationToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Storage Location {(maybePantryLocation.HasValue ? "updated" : "added")}.", pantryLocationToEdit.Id));
    }

    private static void Transfer(string formattedName, PantryLocation pantryLocation)
    {
        pantryLocation.Name = formattedName;
    }
}
