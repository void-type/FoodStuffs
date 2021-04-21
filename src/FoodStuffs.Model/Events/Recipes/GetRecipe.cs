using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Recipes
{
    public static class GetRecipe
    {
        public class Handler : EventHandlerAbstract<Request, RecipeDto>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override Task<IResult<RecipeDto>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new RecipesByIdWithCategoriesAndImagesSpecification(request.Id);

                return _data.Recipes.Get(byId, cancellationToken)
                    .ToResultAsync(new RecipeNotFoundFailure())
                    .SelectAsync(r => new RecipeDto(
                       Id: r.Id,
                       Name: r.Name,
                       Ingredients: r.Ingredients,
                       Directions: r.Directions,
                       CookTimeMinutes: r.CookTimeMinutes,
                       PrepTimeMinutes: r.PrepTimeMinutes,
                       CreatedBy: r.CreatedBy,
                       CreatedOn: r.CreatedOn,
                       ModifiedBy: r.ModifiedBy,
                       ModifiedOn: r.ModifiedOn,
                       PinnedImageId: r.PinnedImageId,
                       Categories: r.CategoryRecipes.Select(cr => cr.Category.Name).OrderBy(n => n),
                       Images: r.Images.Select(i => i.Id)));
            }
        }

        public record Request(int Id);

        public record RecipeDto(
            int Id,
            string Name,
            string Ingredients,
            string Directions,
            int? CookTimeMinutes,
            int? PrepTimeMinutes,
            string CreatedBy,
            DateTime CreatedOn,
            string ModifiedBy,
            DateTime ModifiedOn,
            int? PinnedImageId,
            IEnumerable<string> Categories,
            IEnumerable<int> Images);

        public class RequestLogger : RequestLoggerAbstract<Request>
        {
            public RequestLogger(ILogger<RequestLogger> logger) : base(logger) { }

            public override void Log(Request request)
            {
                Logger.LogInformation("Requested. RecipeId: {RecipeId}",
                    request.Id);
            }
        }

        public class ResponseLogger : FallibleEventLoggerAbstract<Request, RecipeDto>
        {
            public ResponseLogger(ILogger<ResponseLogger> logger) : base(logger) { }

            protected override void OnSuccess(Request request, RecipeDto response)
            {
                Logger.LogInformation("Responded with {ResponseType}. RecipeId: {RecipeId}",
                    nameof(RecipeDto),
                    response.Id);

                base.OnSuccess(request, response);
            }
        }

        public class Pipeline : EventPipelineAbstract<Request, RecipeDto>
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
