using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryDepartments.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.GroceryDepartments;

public class GetGroceryDepartmentHandler : CustomEventHandlerAbstract<GetGroceryDepartmentRequest, GetGroceryDepartmentResponse>
{
    private readonly FoodStuffsContext _data;

    public GetGroceryDepartmentHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetGroceryDepartmentResponse>> Handle(GetGroceryDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new GroceryDepartmentsWithAllRelatedSpecification(request.Id);

        return await _data.GroceryDepartments
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryDepartmentNotFoundFailure())
            .SelectAsync(m => new GetGroceryDepartmentResponse(
                Id: m.Id,
                Name: m.Name,
                Order: m.Order,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                GroceryItems: [.. m.GroceryItems
                    .OrderBy(r => r.Name)
                    .Select(r => new GetGroceryDepartmentResponseGroceryItem(
                        Id: r.Id,
                        Name: r.Name))]));
    }
}
