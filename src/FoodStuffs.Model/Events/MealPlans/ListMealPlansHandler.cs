using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.MealPlans;

public class ListMealPlansHandler : CustomEventHandlerAbstract<ListMealPlansRequest, IItemSet<ListMealPlansResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListMealPlansHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override Task<IResult<IItemSet<ListMealPlansResponse>>> Handle(ListMealPlansRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new MealPlansSpecification();

        return _data.MealPlans
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(ms => new ListMealPlansResponse(
                Id: ms.Id,
                Name: ms.Name,
                CreatedOn: ms.CreatedOn,
                ModifiedOn: ms.ModifiedOn))
            .MapAsync(Ok);
    }
}
