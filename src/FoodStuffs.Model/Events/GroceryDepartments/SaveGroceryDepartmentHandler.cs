using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryDepartments.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.GroceryDepartments;

public class SaveGroceryDepartmentHandler : CustomEventHandlerAbstract<SaveGroceryDepartmentRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveGroceryDepartmentRequestValidator _validator;

    public SaveGroceryDepartmentHandler(FoodStuffsContext data, SaveGroceryDepartmentRequestValidator validator)
    {
        _data = data;
        _validator = validator;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveGroceryDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveGroceryDepartmentRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new GroceryDepartmentsSpecification(request.Id);

        var maybeGroceryDepartment = await _data.GroceryDepartments
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .Include(x => x.ShoppingItems)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new GroceryDepartmentsSpecification(requestedName);

        var conflictingGroceryDepartment = await _data.GroceryDepartments
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingGroceryDepartment is not null && conflictingGroceryDepartment.Id != request.Id)
        {
            return Fail(new Failure("Grocery Department name already exists.", "name"));
        }

        var groceryDepartmentToEdit = maybeGroceryDepartment.Unwrap(() => new GroceryDepartment());

        Transfer(request, requestedName, groceryDepartmentToEdit);

        if (maybeGroceryDepartment.HasValue)
        {
            _data.GroceryDepartments.Update(groceryDepartmentToEdit);
        }
        else
        {
            _data.GroceryDepartments.Add(groceryDepartmentToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Grocery Department {(maybeGroceryDepartment.HasValue ? "updated" : "added")}.", groceryDepartmentToEdit.Id));
    }

    private static void Transfer(SaveGroceryDepartmentRequest request, string formattedName, GroceryDepartment groceryDepartment)
    {
        groceryDepartment.Name = formattedName;
        groceryDepartment.Order = request.Order;
    }
}
