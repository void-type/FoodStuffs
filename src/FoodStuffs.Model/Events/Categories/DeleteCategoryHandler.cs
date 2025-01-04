using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Categories.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Categories;

public class DeleteCategoryHandler : CustomEventHandlerAbstract<DeleteCategoryRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeleteCategoryHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.Categories
            .TagWith(GetTag())
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new CategoryNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                _data.Categories.Remove(r);
                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Category deleted.", r.Id));
    }
}
