using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class PinImageHandler : EventHandlerAbstract<PinImageRequest, EntityMessage<string>>
{
    private readonly IFoodStuffsData _data;
    private readonly IRecipeIndexService _indexService;

    public PinImageHandler(IFoodStuffsData data, IRecipeIndexService indexService)
    {
        _data = data;
        _indexService = indexService;
    }

    public override Task<IResult<EntityMessage<string>>> Handle(PinImageRequest request, CancellationToken cancellationToken = default)
    {
        return _data.Images.Get(new ImagesByNameWithRecipesSpecification(request.Name), cancellationToken)
            .ToResultAsync(new ImageNotFoundFailure())
            .TeeOnSuccessAsync(i => i.Recipe.PinnedImageId = i.Id)
            .SelectAsync(i => i.Recipe)
            .TeeOnSuccessAsync(r => _data.Recipes.Update(r, cancellationToken))
            .TeeOnSuccessAsync(async r => await _indexService.AddOrUpdate(r.Id, cancellationToken))
            .SelectAsync(_ => EntityMessage.Create("Image pinned.", request.Name));
    }
}
