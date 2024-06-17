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
            .SelectAsync(m => new GetMealPlanResponse(
                Id: m.Id,
                Name: m.Name,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                PantryShoppingItems: m.PantryShoppingItemRelations
                    .ConvertAll(i =>
                        new GetMealPlanResponsePantryShoppingItem(i.ShoppingItem.Id, i.ShoppingItem.Name, i.Quantity)
                    ),
                Recipes: m.RecipeRelations
                    .ConvertAll(rel => new GetMealPlanResponseRecipe(
                        Id: rel.Recipe.Id,
                        Name: rel.Recipe.Name,
                        Order: rel.Order,
                        Image: rel.Recipe.DefaultImage?.FileName,
                        Categories: rel.Recipe.Categories
                            .Select(c => c.Name)
                            .OrderBy(n => n)
                            .ToList(),
                        ShoppingItems: rel.Recipe.ShoppingItemRelations
                            // TODO: all apis, order on client (property) or server (implicit)?
                            .OrderBy(i => i.Order)
                            .Select(i =>
                                new GetMealPlanResponseShoppingItem(i.ShoppingItem.Id, i.ShoppingItem.Name, i.Quantity)
                            )
                            .ToList()))
));
    }
}
