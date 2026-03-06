using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealPlans;

internal static class MealPlanQueryHelper
{
    public static async Task<Maybe<MealPlan>> FindWithAllRelatedAsync(
        FoodStuffsContext data,
        int id,
        string tag,
        CancellationToken cancellationToken)
    {
        var byId = new MealPlansWithAllRelatedSpecification(id);

        return await data.MealPlans
            .TagWith(tag)
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);
    }

    public static async Task<MealPlan> GetWithAllRelatedAsync(
        FoodStuffsContext data,
        int id,
        string tag,
        CancellationToken cancellationToken)
    {
        var byId = new MealPlansWithAllRelatedSpecification(id);

        return await data.MealPlans
            .TagWith(tag)
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstAsync(cancellationToken);
    }
}
