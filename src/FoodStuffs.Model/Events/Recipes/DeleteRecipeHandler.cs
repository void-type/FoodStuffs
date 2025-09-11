using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class DeleteRecipeHandler : CustomEventHandlerAbstract<DeleteRecipeRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _recipeIndex;
    private readonly IGroceryItemIndexService _groceryItemIndex;

    public DeleteRecipeHandler(FoodStuffsContext data, IRecipeIndexService recipeIndex, IGroceryItemIndexService groceryItemIndex)
    {
        _data = data;
        _recipeIndex = recipeIndex;
        _groceryItemIndex = groceryItemIndex;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesSpecification(request.Id);

        return await _data.Recipes
            .TagWith(GetTag(byId))
            .ApplyEfSpecification(byId)
            .Include(r => r.GroceryItemRelations)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new RecipeNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                var groceryItemIds = r.GroceryItemRelations.Select(gr => gr.GroceryItemId).ToList();

                _data.Recipes.Remove(r);

                await _data.SaveChangesAsync(cancellationToken);

                _recipeIndex.Remove(r.Id);

                await _groceryItemIndex.AddOrUpdateAsync(groceryItemIds, cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Recipe deleted.", r.Id));
    }
}
