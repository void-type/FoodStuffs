using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.MealPlans.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealPlans;

public class GetMealPlanHandler : CustomEventHandlerAbstract<GetMealPlanRequest, GetMealPlanResponse>
{
    private readonly FoodStuffsContext _data;

    public GetMealPlanHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetMealPlanResponse>> Handle(GetMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealPlansWithAllRelatedSpecification(request.Id);

        return await _data.MealPlans
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new MealPlanNotFoundFailure())
            .SelectAsync(m => m.ToGetMealPlanResponse());
    }
}
