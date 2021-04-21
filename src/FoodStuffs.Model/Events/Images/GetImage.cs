using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Files;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Images
{
    public static class GetImage
    {
        public class Handler : EventHandlerAbstract<Request, SimpleFile>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override Task<IResult<SimpleFile>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new ImagesByIdWithBlobsSpecification(request.Id);

                return _data.Images.Get(byId, cancellationToken)
                    .ToResultAsync(new ImageNotFoundFailure())
                    .SelectAsync(r => new SimpleFile(r.Blob.Bytes, $"{r.Id}"));
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

        public class ResponseLogger : SimpleFileEventLogger<Request>
        {
            public ResponseLogger(ILogger<ResponseLogger> logger) : base(logger) { }
        }

        public class Pipeline : EventPipelineAbstract<Request, SimpleFile>
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
