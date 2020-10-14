using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
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

        public class Request
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Logger : SimpleFileEventLogger<Request>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<SimpleFile> result)
            {
                Logger.Info($"RequestImageId: {request.Id}");
                base.OnBoth(request, result);
            }
        }
    }
}
