using AutoMapper;
using AutoMapper.QueryableExtensions;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System.Collections.Generic;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Queries;
using VoidCore.Model.Responses.ItemSet;

namespace FoodStuffs.Model.DomainEvents.Recipes
{
    public class ListRecipes
    {
        public class Handler : DomainEventAbstract<Request, IItemSetPage<RecipeListItemDto>>
        {
            public Handler(IFoodStuffsData data, IMapper mapper)
            {
                _data = data;
                _mapper = mapper;
            }

            protected override Result<IItemSetPage<RecipeListItemDto>> HandleInternal(Request request)
            {
                var page = _data.Recipes.Stored
                    .ProjectTo<RecipeListItemDto>(_mapper.ConfigurationProvider)
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
            private readonly IMapper _mapper;
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
            public int Id { get; set; }
            public string Name { get; set; }
            public IEnumerable<string> Categories { get; set; }
        }

        public class Logging : ItemSetPageLogging<Request, RecipeListItemDto>
        {
            public Logging(ILoggingService logger) : base(logger) { }

            public override void OnBoth(Request request, IResult<IItemSetPage<RecipeListItemDto>> result)
            {
                Logger.Info($"CategorySearch: '{request.CategorySearch}'", $"NameSearch: '{request.NameSearch}", $"Sort: '{request.Sort}'");
                base.OnBoth(request, result);
            }
        }
    }
}
