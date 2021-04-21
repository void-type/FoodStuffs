using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Recipes
{
    public static class DeleteRecipe
    {
        public class Handler : EventHandlerAbstract<Request, EntityMessage<int>>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override Task<IResult<EntityMessage<int>>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new RecipesByIdWithCategoriesAndImagesSpecification(request.Id);

                return _data.Recipes.Get(byId, cancellationToken)
                    .ToResultAsync(new RecipeNotFoundFailure())
                    .TeeOnSuccessAsync(r => RemoveImages(r, cancellationToken))
                    .TeeOnSuccessAsync(r => _data.CategoryRecipes.RemoveRange(r.CategoryRecipes, cancellationToken))
                    .TeeOnSuccessAsync(r => _data.Recipes.Remove(r, cancellationToken))
                    .SelectAsync(r => EntityMessage.Create("Recipe deleted.", r.Id));
            }

            private async Task RemoveImages(Recipe recipe, CancellationToken cancellationToken)
            {
                var images = recipe.Images;
                // Optimization: don't bring the whole blob into RAM.
                var blobs = images.Select(i => new Blob { Id = i.Id });

                await _data.Blobs.RemoveRange(blobs, cancellationToken).ConfigureAwait(false);
                await _data.Images.RemoveRange(images, cancellationToken).ConfigureAwait(false);
            }
        }

        public record Request(int Id);

        public class RequestLogger : RequestLoggerAbstract<Request>
        {
            public RequestLogger(ILogger<RequestLogger> logger) : base(logger) { }

            public override void Log(Request request)
            {
                Logger.LogInformation("Requested. RecipeId: {RecipeId}",
                    request.Id);
            }
        }

        public class ResponseLogger : EntityMessageEventLogger<Request, int>
        {
            public ResponseLogger(ILogger<ResponseLogger> logger) : base(logger) { }
        }

        public class Pipeline : EventPipelineAbstract<Request, EntityMessage<int>>
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
