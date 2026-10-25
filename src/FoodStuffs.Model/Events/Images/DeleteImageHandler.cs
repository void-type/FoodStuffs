using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Images.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class DeleteImageHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<DeleteImageRequest, EntityMessage<string>>
{
    public override async Task<IResult<EntityMessage<string>>> Handle(DeleteImageRequest request, CancellationToken cancellationToken = default)
    {
        return await data.Images
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

                data.Remove(i);

                await data.SaveChangesAsync(cancellationToken);

                await searchIndex.UpdateAsync(SearchIndex.Recipes, i.RecipeId, cancellationToken);
            })
            .SelectAsync(_ => EntityMessage.Create("Image deleted.", request.Name));
    }
}
