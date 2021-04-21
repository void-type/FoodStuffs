using FoodStuffs.Model.Data;
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
    public static class PinImage
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
                return _data.Images.Get(new ImagesByIdWithRecipesSpecification(request.Id), cancellationToken)
                    .ToResultAsync(new ImageNotFoundFailure())
                    .SelectAsync(i => i.Recipe)
                    .TeeOnSuccessAsync(r => r.PinnedImageId = request.Id)
                    .TeeOnSuccessAsync(r => _data.Recipes.Update(r, cancellationToken))
                    .SelectAsync(_ => EntityMessage.Create("Image pinned.", request.Id));
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
