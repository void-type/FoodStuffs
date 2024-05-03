using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
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

    public override Task<IResult<GetMealPlanResponse>> Handle(GetMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealPlansWithAllRelatedSpecification(request.Id);

        return _data.MealPlans
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new MealPlanNotFoundFailure())
            .SelectAsync(r => new GetMealPlanResponse(
                Id: r.Id,
                Name: r.Name,
                CreatedBy: r.CreatedBy,
                CreatedOn: r.CreatedOn,
                ModifiedBy: r.ModifiedBy,
                ModifiedOn: r.ModifiedOn,
                PantryIngredients: r.PantryIngredients.Select(i => new GetMealPlanResponsePantryIngredient(i.Name, i.Quantity)),
                Recipes: r.Recipes.Select(r => new GetMealPlanResponseRecipe(
                    Id: r.Id,
                    Name: r.Name,
                    Image: r.DefaultImage?.FileName,
                    Categories: r.Categories
                        .Select(c => c.Name)
                        .OrderBy(n => n),
                    Ingredients: r.Ingredients
                        .Select(i => new GetMealPlanResponseRecipeIngredient(i.Name, i.Quantity, i.Order, i.IsCategory))
                        .OrderBy(i => i.Order)))));
    }
}
