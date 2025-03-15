using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.MealPlans.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans;

public class ListMealPlansHandler : CustomEventHandlerAbstract<ListMealPlansRequest, IItemSet<ListMealPlansResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListMealPlansHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListMealPlansResponse>>> Handle(ListMealPlansRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        return await _data.MealPlans
            .TagWith(GetTag())
            .Include(mp => mp.RecipeRelations)
            .OrderByDescending(mp => mp.CreatedOn)
            .Select(mp => new
            {
                MealPlan = mp,
                RecipeCount = mp.RecipeRelations.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(x => new ListMealPlansResponse(
                Id: x.MealPlan.Id,
                Name: x.MealPlan.Name,
                CreatedOn: x.MealPlan.CreatedOn,
                ModifiedOn: x.MealPlan.ModifiedOn,
                RecipeCount: x.RecipeCount))
            .MapAsync(Ok);
    }
}
