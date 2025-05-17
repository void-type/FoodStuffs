using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.StorageLocations.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.StorageLocations;

public class SaveStorageLocationHandler : CustomEventHandlerAbstract<SaveStorageLocationRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveStorageLocationRequestValidator _validator;

    public SaveStorageLocationHandler(FoodStuffsContext data, SaveStorageLocationRequestValidator validator)
    {
        _data = data;
        _validator = validator;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveStorageLocationRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveStorageLocationRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new StorageLocationsWithAllRelatedSpecification(request.Id);

        var maybeStorageLocation = await _data.StorageLocations
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new StorageLocationsSpecification(requestedName);

        var conflictingStorageLocation = await _data.StorageLocations
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingStorageLocation is not null && conflictingStorageLocation.Id != request.Id)
        {
            return Fail(new Failure("Storage location name already exists.", "name"));
        }

        var storageLocationToEdit = maybeStorageLocation.Unwrap(() => new StorageLocation());

        Transfer(requestedName, storageLocationToEdit);

        if (maybeStorageLocation.HasValue)
        {
            _data.StorageLocations.Update(storageLocationToEdit);
        }
        else
        {
            _data.StorageLocations.Add(storageLocationToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Storage Location {(maybeStorageLocation.HasValue ? "updated" : "added")}.", storageLocationToEdit.Id));
    }

    private static void Transfer(string formattedName, StorageLocation storageLocation)
    {
        storageLocation.Name = formattedName;
    }
}
