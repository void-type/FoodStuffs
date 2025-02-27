﻿using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Categories.Models;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Categories;

public class AddCategoryToAllRecipesHandler : CustomEventHandlerAbstract<AddCategoryToAllRecipesRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;

    public AddCategoryToAllRecipesHandler(FoodStuffsContext data, IRecipeIndexService index)
    {
        _data = data;
        _index = index;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(AddCategoryToAllRecipesRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.Categories
            .TagWith(GetTag())
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new CategoryNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                var recipesToAdd = await _data.Recipes
                    .TagWith(GetTag())
                    .Include(r => r.Categories)
                    .Where(r => r.Categories.All(cat => cat.Id != c.Id))
                    .ToListAsync(cancellationToken);

                recipesToAdd.ForEach(r => r.Categories.Add(c));

                await _data.SaveChangesAsync(cancellationToken);

                await _index.RebuildAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Category added to all recipes.", r.Id));
    }
}
