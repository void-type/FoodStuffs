using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryDepartments.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryDepartments;

public class DeleteGroceryDepartmentHandler : CustomEventHandlerAbstract<DeleteGroceryDepartmentRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeleteGroceryDepartmentHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteGroceryDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.GroceryDepartments
            .TagWith(GetTag())
            .Include(c => c.ShoppingItems)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryDepartmentNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                _data.GroceryDepartments.Remove(c);

                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Grocery department deleted.", r.Id));
    }
}
