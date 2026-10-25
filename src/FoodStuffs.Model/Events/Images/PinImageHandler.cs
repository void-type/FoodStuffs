using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Images.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class PinImageHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<PinImageRequest, EntityMessage<string>>
{
    public override async Task<IResult<EntityMessage<string>>> Handle(PinImageRequest request, CancellationToken cancellationToken = default)
    {
        return await data.Images
            .TagWith(GetTag())
            .Include(x => x.Recipe)
            .FirstOrDefaultAsync(i => i.FileName == request.Name, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new ImageNotFoundFailure())
            .TeeOnSuccessAsync(async i =>
            {
                i.Recipe.PinnedImageId = i.Id;

                await data.SaveChangesAsync(cancellationToken);

                await searchIndex.UpdateAsync(SearchIndex.Recipes, i.Recipe.Id, cancellationToken);
            })
            .SelectAsync(_ => EntityMessage.Create("Image pinned.", request.Name));
    }
}
