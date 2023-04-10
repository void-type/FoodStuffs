using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class DeleteRecipeHandler : EventHandlerAbstract<DeleteRecipeRequest, EntityMessage<int>>
{
    private readonly IFoodStuffsData _data;

    public DeleteRecipeHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override Task<IResult<EntityMessage<int>>> Handle(DeleteRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesByIdSpecification(request.Id);

        return _data.Recipes.Get(byId, cancellationToken)
            .ToResultAsync(new RecipeNotFoundFailure())
            .TeeOnSuccessAsync(r => _data.Recipes.Remove(r, cancellationToken))
            .SelectAsync(r => EntityMessage.Create("Recipe deleted.", r.Id));
    }
}
