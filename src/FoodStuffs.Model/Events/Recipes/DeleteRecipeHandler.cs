using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class DeleteRecipeHandler : EventHandlerAbstract<DeleteRecipeRequest, EntityMessage<int>>
{
    private readonly IFoodStuffsData _data;
    private readonly IRecipeIndexService _indexService;

    public DeleteRecipeHandler(IFoodStuffsData data, IRecipeIndexService indexService)
    {
        _data = data;
        _indexService = indexService;
    }

    public override Task<IResult<EntityMessage<int>>> Handle(DeleteRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesByIdSpecification(request.Id);

        return _data.Recipes.Get(byId, cancellationToken)
            .ToResultAsync(new RecipeNotFoundFailure())
            .TeeOnSuccessAsync(r => _data.Recipes.Remove(r, cancellationToken))
            .TeeOnSuccessAsync(r => _indexService.Remove(r.Id))
            .SelectAsync(r => EntityMessage.Create("Recipe deleted.", r.Id));
    }
}
