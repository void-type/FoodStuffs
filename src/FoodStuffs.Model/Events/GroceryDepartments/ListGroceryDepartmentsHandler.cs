using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryDepartments.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryDepartments;

public class ListGroceryDepartmentsHandler : CustomEventHandlerAbstract<ListGroceryDepartmentsRequest, IItemSet<ListGroceryDepartmentsResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListGroceryDepartmentsHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListGroceryDepartmentsResponse>>> Handle(ListGroceryDepartmentsRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new GroceryDepartmentsSpecification(request);

        return await _data.GroceryDepartments
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .Select(gd => new
            {
                GroceryDepartment = gd,
                GroceryItemCount = gd.GroceryItems.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(x => new ListGroceryDepartmentsResponse(
                Id: x.GroceryDepartment.Id,
                Name: x.GroceryDepartment.Name,
                Order: x.GroceryDepartment.Order,
                GroceryItemCount: x.GroceryItemCount))
            .MapAsync(Ok);
    }
}
