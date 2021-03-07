using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
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

        public class Logger : EntityMessageEventLogger<Request, int>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<EntityMessage<int>> result)
            {
                Logger.Info($"RequestImageId: {request.Id}");
                base.OnBoth(request, result);
            }
        }
    }
}
