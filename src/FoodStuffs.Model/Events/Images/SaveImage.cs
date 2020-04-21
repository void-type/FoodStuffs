using System.Threading;
using System.Threading.Tasks;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images
{
    public class SaveImage
    {
        public class Handler : EventHandlerAbstract<Request, EntityMessage<int>>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<EntityMessage<int>>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                // Note: Size limit is controlled by the server (IIS and/or Kestrel) and validated on the client. Default is 30MB (~28.6 MiB).
                // To change this, you will need both:
                // 1. a web.config with requestLimits maxAllowedContentLength="<byte size>"
                // 2. configure FormOptions in startup for options.MultipartBodyLengthLimit = <byte size>
                // 3. edit the client-side upload validation in the recipeedit.vue file.

                var recipeResult = await _data.Recipes.Get(new RecipesByIdSpecification(request.RecipeId), cancellationToken)
                    .ToResultAsync(new RecipeNotFoundFailure());

                if (recipeResult.IsFailed)
                {
                    return Fail(recipeResult.Failures);
                }

                var image = new Image
                {
                    RecipeId = recipeResult.Value.Id
                };

                await _data.Images.Add(image, cancellationToken);

                var blob = new Blob
                {
                    Id = image.Id,
                    Bytes = request.FileContent
                };

                await _data.Blobs.Add(blob, cancellationToken);

                return Ok(EntityMessage.Create("Image uploaded.", image.Id));
            }
        }

        public class Request
        {
            public Request(int recipeId, byte[] fileContent)
            {
                RecipeId = recipeId;
                FileContent = fileContent;
            }

            public int RecipeId { get; }
            public byte[] FileContent { get; }
        }

        public class Logger : EntityMessageEventLogger<Request, int>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<EntityMessage<int>> result)
            {
                Logger.Info(
                    $"RequestRecipeId: {request.RecipeId}",
                    $"RequestFileSize: {request.FileContent.Length}");
                base.OnBoth(request, result);
            }
        }
    }
}
