using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class SaveImageHandler : EventHandlerAbstract<SaveImageRequest, EntityMessage<int>>
{
    private readonly IFoodStuffsData _data;

    public SaveImageHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveImageRequest request, CancellationToken cancellationToken = default)
    {
        // Note: Size limit is controlled by the server (IIS and/or Kestrel) and validated on the client. Default is 30MB (~28.6 MiB).
        // To change this, you will need both:
        // 1. a web.config with requestLimits maxAllowedContentLength="<byte size>"
        // 2. configure FormOptions in startup for options.MultipartBodyLengthLimit = <byte size>
        // 3. edit the client-side upload validation in the recipeedit.vue file.

        var recipeResult = await _data.Recipes.Get(new RecipesByIdSpecification(request.RecipeId), cancellationToken)
            .ToResultAsync(new RecipeNotFoundFailure()).ConfigureAwait(false);

        if (recipeResult.IsFailed)
        {
            return Fail(recipeResult.Failures);
        }

        var image = new Image
        {
            RecipeId = recipeResult.Value.Id
        };

        await _data.Images.Add(image, cancellationToken).ConfigureAwait(false);

        var blob = new Blob
        {
            Id = image.Id,
            Bytes = request.FileContent
        };

        await _data.Blobs.Add(blob, cancellationToken).ConfigureAwait(false);

        return Ok(EntityMessage.Create("Image uploaded.", image.Id));
    }
}
