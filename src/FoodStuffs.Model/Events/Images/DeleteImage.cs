using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Images
{
    public static class DeleteImage
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
                var byId = new ImagesByIdWithRecipesSpecification(request.Id);

                return _data.Images.Get(byId, cancellationToken)
                    .ToResultAsync(new ImageNotFoundFailure())
                    .TeeOnSuccessAsync(async i =>
                    {
                        if (i.Recipe.PinnedImageId == i.Id)
                        {
                            i.Recipe.PinnedImageId = null;
                            await _data.Recipes.Update(i.Recipe, cancellationToken).ConfigureAwait(false);
                        }
                    })
                    .TeeOnSuccessAsync(i => _data.Blobs.Remove(new Blob { Id = i.Id }, cancellationToken))
                    .TeeOnSuccessAsync(i => _data.Images.Remove(i, cancellationToken))
                    .SelectAsync(i => EntityMessage.Create("Image deleted.", i.Id));
            }
        }

        public record Request(int Id);

        public class RequestLogger : RequestLoggerAbstract<Request>
        {
            public RequestLogger(ILogger<RequestLogger> logger) : base(logger) { }

            public override void Log(Request request)
            {
                Logger.LogInformation("Requested. ImageId: {ImageId}",
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
