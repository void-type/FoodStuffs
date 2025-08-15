using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Categories.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.Categories;

public class GetCategoryHandler : CustomEventHandlerAbstract<GetCategoryRequest, GetCategoryResponse>
{
    private readonly FoodStuffsContext _data;

    public GetCategoryHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetCategoryResponse>> Handle(GetCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new CategoriesWithAllRelatedSpecification(request.Id);

        return await _data.Categories
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new CategoryNotFoundFailure())
            .SelectAsync(m => new GetCategoryResponse(
                Id: m.Id,
                Name: m.Name,
                ShowInMealPlan: m.ShowInMealPlan,
                Color: m.Color,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                Recipes: [.. m.Recipes
                    .OrderBy(r => r.Name)
                    .Select(r => new GetCategoryResponseRecipe(
                        Id: r.Id,
                        Name: r.Name,
                        Slug: r.Slug,
                        Image: r.DefaultImage?.FileName))]));
    }
}
