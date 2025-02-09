using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Categories.Models;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.Categories;

public class SaveCategoryHandler : CustomEventHandlerAbstract<SaveCategoryRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveCategoryRequestValidator _validator;
    private readonly IRecipeIndexService _index;

    public SaveCategoryHandler(FoodStuffsContext data, SaveCategoryRequestValidator validator, IRecipeIndexService index)
    {
        _data = data;
        _validator = validator;
        _index = index;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveCategoryRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(request.Name).Trim();

        var byId = new CategoriesSpecification(request.Id);

        var maybeCategory = await _data.Categories
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .Include(c => c.Recipes)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        // Check for conflicting items by name
        var byName = new CategoriesSpecification(requestedName);

        var conflictingCategory = await _data.Categories
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingCategory is not null && conflictingCategory.Id != request.Id)
        {
            return Fail(new Failure("Category name already exists.", "name"));
        }

        var categoryToEdit = maybeCategory.Unwrap(() => new Category());

        Transfer(requestedName, categoryToEdit);

        if (maybeCategory.HasValue)
        {
            _data.Categories.Update(categoryToEdit);

            foreach (var id in categoryToEdit.Recipes.ConvertAll(r => r.Id))
            {
                await _index.AddOrUpdateAsync(id, cancellationToken);
            }
        }
        else
        {
            await _data.Categories.AddAsync(categoryToEdit, cancellationToken);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Category {(maybeCategory.HasValue ? "updated" : "added")}.", categoryToEdit.Id));
    }

    private static void Transfer(string formattedName, Category category)
    {
        category.Name = formattedName;
    }
}
