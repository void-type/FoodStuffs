using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.Recipes;

public class GetRecipeHandler : CustomEventHandlerAbstract<GetRecipeRequest, GetRecipeResponse>
{
    private readonly FoodStuffsContext _data;

    public GetRecipeHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override Task<IResult<GetRecipeResponse>> Handle(GetRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesWithAllRelatedSpecification(request.Id);

        return _data.Recipes
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new RecipeNotFoundFailure())
            .SelectAsync(r => new GetRecipeResponse(
                Id: r.Id,
                Name: r.Name,
                Directions: r.Directions,
                PrepTimeMinutes: r.PrepTimeMinutes,
                CookTimeMinutes: r.CookTimeMinutes,
                IsForMealPlanning: r.IsForMealPlanning,
                CreatedBy: r.CreatedBy,
                CreatedOn: r.CreatedOn,
                ModifiedBy: r.ModifiedBy,
                ModifiedOn: r.ModifiedOn,
                Slug: r.Slug,
                DefaultImage: r.DefaultImage?.FileName,
                PinnedImage: r.PinnedImage?.FileName,
                Images: r.Images
                    .ConvertAll(i => i.FileName),
                Categories: r.Categories
                    .Select(c => c.Name)
                    .OrderBy(n => n)
                    .ToList(),
                Ingredients: r.Ingredients
                    .Select(i => new GetRecipeResponseIngredient(
                        Name: i.Name,
                        Quantity: i.Quantity,
                        Order: i.Order,
                        IsCategory: i.IsCategory))
                    .OrderBy(i => i.Order)
                    .ToList(),
                ShoppingItems: r.ShoppingItemRelations
                    .Select(i => new GetRecipeResponseShoppingItem(
                        Id: i.ShoppingItem.Id,
                        Name: i.ShoppingItem.Name,
                        Quantity: i.Quantity,
                        Order: i.Order))
                    .OrderBy(i => i.Order)
                    .ToList()
                ));
    }
}
