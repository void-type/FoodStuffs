using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryDepartments.Models;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryDepartments;

public class DeleteGroceryDepartmentHandler : CustomEventHandlerAbstract<DeleteGroceryDepartmentRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;

    public DeleteGroceryDepartmentHandler(FoodStuffsContext data, IRecipeIndexService index)
    {
        _data = data;
        _index = index;
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
                var shoppingItems = c.ShoppingItems.ConvertAll(r => r.Id);

                _data.GroceryDepartments.Remove(c);
                await _data.SaveChangesAsync(cancellationToken);

                foreach (var id in shoppingItems)
                {
                    await _index.AddOrUpdateAsync(id, cancellationToken);
                }
            })
            .SelectAsync(r => EntityMessage.Create("Grocery Department deleted.", r.Id));
    }
}
