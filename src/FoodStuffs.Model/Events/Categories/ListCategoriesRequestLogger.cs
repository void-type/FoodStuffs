﻿using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;

namespace FoodStuffs.Model.Events.Categories;

public class ListCategoriesRequestLogger : RequestLoggerAbstract<ListCategoriesRequest>
{
    public ListCategoriesRequestLogger(ILogger<ListCategoriesRequestLogger> logger) : base(logger) { }

    public override void Log(ListCategoriesRequest request)
    {
        Logger.LogInformation("Requested. NameSearch: {NameSearch} IsPagingEnabled: {IsPagingEnabled} Page: {Page} Take: {Take}",
            request.NameSearch,
            request.IsPagingEnabled,
            request.Page,
            request.Take);
    }
}
