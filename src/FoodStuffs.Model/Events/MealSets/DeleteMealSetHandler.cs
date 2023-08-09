using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class DeleteMealSetHandler : EventHandlerAbstract<DeleteMealSetRequest, EntityMessage<int>>
{
    private readonly IFoodStuffsData _data;

    public DeleteMealSetHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override Task<IResult<EntityMessage<int>>> Handle(DeleteMealSetRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealSetsByIdSpecification(request.Id);

        return _data.MealSets.Get(byId, cancellationToken)
            .ToResultAsync(new MealSetNotFoundFailure())
            .TeeOnSuccessAsync(r => _data.MealSets.Remove(r, cancellationToken))
            .SelectAsync(r => EntityMessage.Create("Meal set deleted.", r.Id));
    }
}
