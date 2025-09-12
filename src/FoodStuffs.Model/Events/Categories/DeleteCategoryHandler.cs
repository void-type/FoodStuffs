using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Categories.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Categories;

public class DeleteCategoryHandler : CustomEventHandlerAbstract<DeleteCategoryRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly ISearchIndexService _searchIndex;

    public DeleteCategoryHandler(FoodStuffsContext data, ISearchIndexService searchIndex)
    {
        _data = data;
        _searchIndex = searchIndex;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.Categories
            .TagWith(GetTag())
            .Include(c => c.Recipes)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new CategoryNotFoundFailure())
            .TeeOnSuccessAsync(async c =>
            {
                var recipeIds = c.Recipes.ConvertAll(r => r.Id);

                _data.Categories.Remove(c);

                await _data.SaveChangesAsync(cancellationToken);

                await _searchIndex.AddOrUpdateAsync(SearchIndex.Recipes, recipeIds, cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Category deleted.", r.Id));
    }
}
