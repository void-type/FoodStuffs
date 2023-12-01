using VoidCore.Model.Events;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.Categories;

public class ListCategoriesPipeline : EventPipelineAbstract<ListCategoriesRequest, IItemSet<ListCategoriesResponse>>
{
    public ListCategoriesPipeline(ListCategoriesHandler handler, ListCategoriesRequestLogger requestLogger, ListCategoriesResponseLogger responseLogger)
    {
        InnerHandler = handler
            .AddRequestLogger(requestLogger)
            .AddPostProcessor(responseLogger);
    }
}
