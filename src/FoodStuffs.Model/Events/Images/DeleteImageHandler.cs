using FoodStuffs.Model.Data;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class DeleteImageHandler : CustomEventHandlerAbstract<DeleteImageRequest, EntityMessage<string>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;

    public DeleteImageHandler(FoodStuffsContext data, IRecipeIndexService index)
    {
        _data = data;
        _index = index;
    }

    public override Task<IResult<EntityMessage<string>>> Handle(DeleteImageRequest request, CancellationToken cancellationToken = default)
    {
        return _data.Images
            .TagWith(GetTag())
            .Include(x => x.Recipe)
            .FirstOrDefaultAsync(i => i.FileName == request.Name, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new ImageNotFoundFailure())
            .TeeOnSuccessAsync(async i =>
            {
                if (i.Recipe.PinnedImageId == i.Id)
                {
                    i.Recipe.PinnedImageId = null;
                }

                _data.Remove(i);

                await _data.SaveChangesAsync(cancellationToken);
                await _index.AddOrUpdate(i.RecipeId, cancellationToken);
            })
            .SelectAsync(i => EntityMessage.Create("Image deleted.", request.Name));
    }
}
