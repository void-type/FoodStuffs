using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Images.Models;
using FoodStuffs.Model.Search.Recipes;
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

    public override async Task<IResult<EntityMessage<string>>> Handle(DeleteImageRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.Images
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
                await _index.AddOrUpdateAsync(i.RecipeId, cancellationToken);
            })
            .SelectAsync(i => EntityMessage.Create("Image deleted.", request.Name));
    }
}
