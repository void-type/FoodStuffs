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
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(c => new ListCategoriesResponse(
                Id: c.Id,
                Name: c.Name))
            .MapAsync(Ok);
    }
}
