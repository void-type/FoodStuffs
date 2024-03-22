using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class DeleteRecipeHandler : CustomEventHandlerAbstract<DeleteRecipeRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;

    public DeleteRecipeHandler(FoodStuffsContext data, IRecipeIndexService index)
    {
        _data = data;
        _index = index;
    }

    public override Task<IResult<EntityMessage<int>>> Handle(DeleteRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesSpecification(request.Id);

        return _data.Recipes
            .TagWith(GetTag(byId))
            .ApplyEfSpecification(byId)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new RecipeNotFoundFailure())
            .TeeOnSuccessAsync(r =>
            {
                _data.Recipes.Remove(r);
                _data.SaveChangesAsync(cancellationToken);
            })
            .TeeOnSuccessAsync(r => _index.Remove(r.Id))
            .SelectAsync(r => EntityMessage.Create("Recipe deleted.", r.Id));
    }
}
