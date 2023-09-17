using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class PinImageHandler : EventHandlerAbstract<PinImageRequest, EntityMessage<string>>
{
    private readonly IFoodStuffsData _data;

    public PinImageHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override Task<IResult<EntityMessage<string>>> Handle(PinImageRequest request, CancellationToken cancellationToken = default)
    {
        return _data.Images.Get(new ImagesByNameWithRecipesSpecification(request.Name), cancellationToken)
            .ToResultAsync(new ImageNotFoundFailure())
            .TeeOnSuccessAsync(i => i.Recipe.PinnedImageId = i.Id)
            .SelectAsync(i => i.Recipe)
            .TeeOnSuccessAsync(r => _data.Recipes.Update(r, cancellationToken))
            .SelectAsync(_ => EntityMessage.Create("Image pinned.", request.Name));
    }
}
