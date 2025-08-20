using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Recipes.Models;
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

    public override async Task<IResult<GetRecipeResponse>> Handle(GetRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesWithAllRelatedSpecification(request.Id);

        return await _data.Recipes
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
                Sides: r.Sides,
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
                Categories: [.. r.Categories
                    .Select(c => new GetRecipeResponseCategory(
                        Id: c.Id,
                        Name: c.Name,
                        Color: c.Color
                    ))
                    .OrderBy(c => c.Name)],
                GroceryItems: [.. r.GroceryItemRelations
                    .Select(i => new GetRecipeResponseGroceryItem(
                        Id: i.GroceryItem.Id,
                        Name: i.GroceryItem.Name,
                        InventoryQuantity: i.GroceryItem.InventoryQuantity,
                        Quantity: i.Quantity,
                        Order: i.Order))
                    .OrderBy(i => i.Order)]
                ));
    }
}
