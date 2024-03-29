﻿using FoodStuffs.Model.Search;
using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Recipes;

public class SearchRecipesRequestLogger : RequestLoggerAbstract<RecipeSearchRequest>
{
    public SearchRecipesRequestLogger(ILogger<SearchRecipesRequestLogger> logger) : base(logger) { }

    public override void Log(RecipeSearchRequest request)
    {
        Logger.LogInformation("Requested. NameSearch: {NameSearch} RequestCategoryIds: {CategoryIds} RequestIsForMealPlanning: {IsForMealPlanning} RequestSort: {SortBy} RequestIsPagingEnabled: {IsPagingEnabled} RequestPage: {Page} RequestTake: {Take}",
            request.NameSearch,
            string.Join(",", request.CategoryIds ?? Enumerable.Empty<int>()),
            request.IsForMealPlanning,
            request.SortBy,
            request.IsPagingEnabled,
            request.Page,
            request.Take);
    }
}
