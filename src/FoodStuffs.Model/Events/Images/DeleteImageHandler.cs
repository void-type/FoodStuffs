using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class DeleteImageHandler : EventHandlerAbstract<DeleteImageRequest, EntityMessage<string>>
{
    private readonly IFoodStuffsData _data;
    private readonly IRecipeIndexService _indexService;

    public DeleteImageHandler(IFoodStuffsData data, IRecipeIndexService indexService)
    {
        _data = data;
        _indexService = indexService;
    }

    public override Task<IResult<EntityMessage<string>>> Handle(DeleteImageRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new ImagesByNameWithRecipesSpecification(request.Name);

        return _data.Images.Get(byId, cancellationToken)
            .ToResultAsync(new ImageNotFoundFailure())
            .TeeOnSuccessAsync(async i =>
            {
                if (i.Recipe.PinnedImageId == i.Id)
                {
                    i.Recipe.PinnedImageId = null;
                    await _data.Recipes.Update(i.Recipe, cancellationToken);
                }
            })
            .TeeOnSuccessAsync(i => _data.Images.Remove(i, cancellationToken))
            .TeeOnSuccessAsync(async i => await _indexService.AddOrUpdate(i.RecipeId, cancellationToken))
            .SelectAsync(i => EntityMessage.Create("Image deleted.", request.Name));
    }
}
