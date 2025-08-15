using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Categories.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Categories;

public class ListCategoriesHandler : CustomEventHandlerAbstract<ListCategoriesRequest, IItemSet<ListCategoriesResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListCategoriesHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListCategoriesResponse>>> Handle(ListCategoriesRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new CategoriesSpecification(request);

        return await _data.Categories
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .Select(c => new
            {
                Category = c,
                RecipeCount = c.Recipes.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(x => new ListCategoriesResponse(
                Id: x.Category.Id,
                Name: x.Category.Name,
                Color: x.Category.Color,
                RecipeCount: x.RecipeCount))
            .MapAsync(Ok);
    }
}
