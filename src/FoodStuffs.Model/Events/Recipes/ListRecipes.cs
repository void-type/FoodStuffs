using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Recipes
{
    public static class ListRecipes
    {
        public class Handler : EventHandlerAbstract<Request, IItemSet<RecipeListItemDto>>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<IItemSet<RecipeListItemDto>>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var paginationOptions = new PaginationOptions(request.Page, request.Take, request.IsPagingEnabled);

                var searchCriteria = GetSearchCriteria(request);

                var allSearch = new RecipesSearchSpecification(searchCriteria);

                var totalCount = await _data.Recipes.Count(allSearch, cancellationToken).ConfigureAwait(false);

                var pagedSearch = new RecipesSearchSpecification(
                    criteria: searchCriteria,
                    paginationOptions: paginationOptions,
                    sortBy: request.SortBy,
                    sortDesc: request.SortDesc);

                var recipes = await _data.Recipes.List(pagedSearch, cancellationToken).ConfigureAwait(false);

                return recipes
                    .Select(r => new RecipeListItemDto(
                        Id: r.Id,
                        Name: r.Name,
                        Categories: r.CategoryRecipes.Select(cr => cr.Category.Name).OrderBy(n => n),
                        ImageId: r.PinnedImageId ?? (r.Images.Count > 0 ? r.Images.Select(i => i.Id).FirstOrDefault() : (int?)null)))
                    .ToItemSet(paginationOptions, totalCount)
                    .Map(Ok);
            }

            private static Expression<Func<Recipe, bool>>[] GetSearchCriteria(Request request)
            {
                var searchCriteria = new List<Expression<Func<Recipe, bool>>>();

                if (!string.IsNullOrWhiteSpace(request.NameSearch))
                {
                    searchCriteria.Add(recipe => recipe.Name.ToLower().Contains(request.NameSearch.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(request.CategorySearch))
                {
                    searchCriteria.Add(recipe => recipe.CategoryRecipes.Any(cr => cr.Category.Name.ToLower().Contains(request.CategorySearch.ToLower())));
                }

                return searchCriteria.ToArray();
            }
        }

        public record Request(
            string? NameSearch,
            string? CategorySearch,
            string? SortBy,
            bool SortDesc,
            bool IsPagingEnabled,
            int Page,
            int Take);

        public record RecipeListItemDto(
            int Id,
            string Name,
            IEnumerable<string> Categories,
            int? ImageId);

        public class RequestLogger : RequestLoggerAbstract<Request>
        {
            public RequestLogger(ILogger<RequestLogger> logger) : base(logger) { }

            public override void Log(Request request)
            {
                Logger.LogInformation("Requested. NameSearch: {NameSearch} RequestCategorySearch: {CategorySearch} RequestSort: {SortBy} RequestIsPagingEnabled: {IsPagingEnabled} RequestPage: {Page} RequestTake: {Take}",
                    request.NameSearch,
                    request.CategorySearch,
                    request.SortBy,
                    request.IsPagingEnabled,
                    request.Page,
                    request.Take);
            }
        }

        public class ResponseLogger : ItemSetEventLogger<Request, RecipeListItemDto>
        {
            public ResponseLogger(ILogger<ResponseLogger> logger) : base(logger) { }
        }

        public class Pipeline : EventPipelineAbstract<Request, IItemSet<RecipeListItemDto>>
        {
            public Pipeline(Handler handler, RequestLogger requestLogger, ResponseLogger responseLogger)
            {
                InnerHandler = handler
                    .AddRequestLogger(requestLogger)
                    .AddPostProcessor(responseLogger);
            }
        }
    }
}
