using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
using VoidCore.Model.Queries;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Domain.Recipes
{
    public class ListRecipes
    {
        public class Handler : EventHandlerAbstract<Request, IItemSetPage<RecipeListItemDto>>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<IItemSetPage<RecipeListItemDto>>> Handle(Request request, CancellationToken cancellationToken = default(CancellationToken))
            {
                var searchExpressions = new []
                {
                    SearchCriteria.PropertiesContain<Recipe>(
                    new SearchTerms(request.NameSearch),
                    r => r.Name
                    ),
                    // TODO: Category search doesn't seem to work against SQL Server.
                    SearchCriteria.PropertiesContain<Recipe>(
                    new SearchTerms(request.CategorySearch),
                    r => string.Join(" ", r.CategoryRecipe.Select(cr => cr.Category.Name))
                    )
                };

                var recipes = await _data.Recipes.List(new RecipesSearchPaginatedSpecification(request, searchExpressions));
                var totalCount = await _data.Recipes.Count(new RecipesSearchSpecification(request, searchExpressions));

                return recipes
                    .Select(recipe => new RecipeListItemDto(
                        recipe.Id,
                        recipe.Name,
                        recipe.CategoryRecipe.Select(cr => cr.Category.Name)))
                    .ToItemSetPage(request.Page, request.Take, totalCount)
                    .Map(page => Result.Ok(page));
            }

            private readonly IFoodStuffsData _data;
        }

        public class Request
        {
            public Request(int page, int take, string nameSearch, string categorySearch, string nameSort)
            {
                Take = take;
                Page = page;
                NameSearch = nameSearch;
                CategorySearch = categorySearch;
                NameSort = nameSort;
            }

            public int Take { get; }
            public int Page { get; }
            public string NameSearch { get; }
            public string CategorySearch { get; }
            public string NameSort { get; }
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

            protected override void OnBoth(Request request, IResult<IItemSetPage<RecipeListItemDto>> result)
            {
                Logger.Info($"CategorySearch: '{request.CategorySearch}'", $"NameSearch: '{request.NameSearch}'", $"NameSort: '{request.NameSort}'");
                base.OnBoth(request, result);
            }
        }
    }
}
