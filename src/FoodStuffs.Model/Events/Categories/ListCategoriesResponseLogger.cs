using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Categories;

public class ListCategoriesResponseLogger : ItemSetEventLogger<ListCategoriesRequest, ListCategoriesResponse>
{
    public ListCategoriesResponseLogger(ILogger<ListCategoriesResponseLogger> logger) : base(logger) { }
}
