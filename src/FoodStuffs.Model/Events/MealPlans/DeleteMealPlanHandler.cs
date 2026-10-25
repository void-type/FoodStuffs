using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.MealPlans.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealPlans;

public class DeleteMealPlanHandler(FoodStuffsContext data) : CustomEventHandlerAbstract<DeleteMealPlanRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        return await data.MealPlans
            .TagWith(GetTag())
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new MealPlanNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                data.MealPlans.Remove(r);

                await data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Meal plan deleted.", r.Id));
    }
}
