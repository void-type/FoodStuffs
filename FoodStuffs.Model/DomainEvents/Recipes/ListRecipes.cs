using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Queries;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.DomainEvents.Recipes
{
    public class ListRecipes
    {
        public class Handler : EventHandlerSyncAbstract<Request, IItemSetPage<RecipeListItemDto>>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            protected override Result<IItemSetPage<RecipeListItemDto>> HandleSync(Request request)
            {
                var page = _data.Recipes.Stored
                    .Select(recipe => new RecipeListItemDto(
                        recipe.Id,
                        recipe.Name,
                        recipe.CategoryRecipe.Select(cr => cr.Category.Name)))
                    .SearchStringProperties(
                        request.NameSearch,
                        dto => dto.Name
                    )
                    .SearchStringProperties(
                        request.CategorySearch,
                        dto => string.Join(" ", dto.Categories)
                    )
                    .SortListItemDtosByName(request.Sort)
                    .ToItemSetPage(request.Page, request.Take);

                return Result.Ok(page);
            }

            private readonly IFoodStuffsData _data;
        }

        public class Request
        {
            public Request(int take, int page, string nameSearch, string categorySearch, string sort)
            {
                Take = take;
                Page = page;
                NameSearch = nameSearch;
                CategorySearch = categorySearch;
                Sort = sort;
            }

            public int Take { get; }
            public int Page { get; }
            public string NameSearch { get; }
            public string CategorySearch { get; }
            public string Sort { get; }
        }

        public class RecipeListItemDto
        {
            public RecipeListItemDto(int id, string name, IEnumerable<string> categories)
            {
                Id = id;
                Name = name;
                Categories = categories;
            }

            public int Id { get; }
            public string Name { get; }
            public IEnumerable<string> Categories { get; }
        }

        public class Logger : ItemSetPageEventLogger<Request, RecipeListItemDto>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            public override void OnBoth(Request request, IResult<IItemSetPage<RecipeListItemDto>> result)
            {
                Logger.Info($"CategorySearch: '{request.CategorySearch}'", $"NameSearch: '{request.NameSearch}", $"Sort: '{request.Sort}'");
                base.OnBoth(request, result);
            }
        }
    }
}
