using FoodStuffs.Model.Data;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class PinImageHandler : CustomEventHandlerAbstract<PinImageRequest, EntityMessage<string>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;

    public PinImageHandler(FoodStuffsContext data, IRecipeIndexService index)
    {
        _data = data;
        _index = index;
    }

    public override Task<IResult<EntityMessage<string>>> Handle(PinImageRequest request, CancellationToken cancellationToken = default)
    {
        return _data.Images
            .TagWith(GetTag())
            .Include(x => x.Recipe)
            .FirstOrDefaultAsync(i => i.FileName == request.Name, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new ImageNotFoundFailure())
            .TeeOnSuccessAsync(async i =>
            {
                i.Recipe.PinnedImageId = i.Id;

                await _data.SaveChangesAsync(cancellationToken);
                await _index.AddOrUpdate(i.Recipe.Id, cancellationToken);
            })
            .SelectAsync(_ => EntityMessage.Create("Image pinned.", request.Name));
    }
}
