using AutoMapper;
using AutoMapper.QueryableExtensions;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;

namespace FoodStuffs.Model.DomainEvents.Recipes
{
    public class GetRecipe
    {
        public class Handler : EventHandlerAbstract<Request, RecipeDto>
        {
            public Handler(IFoodStuffsData data, IMapper mapper)
            {
                _data = data;
                _mapper = mapper;
            }

            protected override Result<RecipeDto> HandleInternal(Request request)
            {
                var dto = _data.Recipes.Stored
                    .WhereById(request.Id)
                    .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefault();

                if (dto == null)
                {
                    return Result.Fail<RecipeDto>("Recipe not found.");
                }

                return Result.Ok(dto);
            }

            private readonly IFoodStuffsData _data;
            private readonly IMapper _mapper;
        }

        public class Request
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class RecipeDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Ingredients { get; set; }
            public string Directions { get; set; }
            public int? CookTimeMinutes { get; set; }
            public int? PrepTimeMinutes { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOnUtc { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime ModifiedOnUtc { get; set; }
            public IEnumerable<string> Categories { get; set; } = new List<string>();
        }

        public class Logger : FallibleEventLogger<Request, RecipeDto>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            public override void OnBoth(Request request, IResult<RecipeDto> result)
            {
                Logger.Info($"Id: '{request.Id}'");
                base.OnBoth(request, result);
            }
        }
    }
}
