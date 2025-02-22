using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.MealPlans.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealPlans;

public class DeleteMealPlanHandler : CustomEventHandlerAbstract<DeleteMealPlanRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeleteMealPlanHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.MealPlans
            .TagWith(GetTag())
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new MealPlanNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                _data.MealPlans.Remove(r);

                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Meal plan deleted.", r.Id));
    }
}
