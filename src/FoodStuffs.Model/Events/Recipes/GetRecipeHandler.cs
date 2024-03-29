﻿using FoodStuffs.Model.Data;
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
               CookTimeMinutes: r.CookTimeMinutes,
               PrepTimeMinutes: r.PrepTimeMinutes,
               CreatedBy: r.CreatedBy,
               CreatedOn: r.CreatedOn,
               ModifiedBy: r.ModifiedBy,
               ModifiedOn: r.ModifiedOn,
               Slug: r.Slug,
               PinnedImage: r.PinnedImage?.FileName,
               IsForMealPlanning: r.IsForMealPlanning,
               Categories: r.Categories
                .Select(c => c.Name)
                .OrderBy(n => n),
               Images: r.Images.Select(i => i.FileName),
               Ingredients: r.Ingredients
                .Select(i => new GetRecipeResponseIngredient(i.Name, i.Quantity, i.Order, i.IsCategory))
                .OrderBy(i => i.Order)));
    }
}
