using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using ImageMagick;
using ImageMagick.Formats;
using Microsoft.Extensions.Logging;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class SaveImageHandler : EventHandlerAbstract<SaveImageRequest, EntityMessage<string>>
{
    private readonly IFoodStuffsData _data;
    private readonly ILogger<SaveImageHandler> _logger;

    public SaveImageHandler(IFoodStuffsData data, ILogger<SaveImageHandler> logger)
    {
        _data = data;
        _logger = logger;
    }

    public override async Task<IResult<EntityMessage<string>>> Handle(SaveImageRequest request, CancellationToken cancellationToken = default)
    {
        // Note: Size limit is controlled by the server (IIS and/or Kestrel) and validated on the client. Default is 30MB (~28.6 MiB).
        // To change this, you will need both:
        // 1. a web.config with requestLimits maxAllowedContentLength="<byte size>"
        // 2. configure FormOptions in startup for options.MultipartBodyLengthLimit = <byte size>
        // 3. edit the client-side upload validation in the RecipeEdit.vue file.

        var recipeResult = await _data.Recipes.Get(new RecipesByIdSpecification(request.RecipeId), cancellationToken)
            .ToResultAsync(new RecipeNotFoundFailure());

        if (recipeResult.IsFailed)
        {
            return Fail(recipeResult.Failures);
        }

        var compressResult = await CompressImage(request.FileContent, 95, 1200, cancellationToken);

        if (compressResult.IsFailed)
        {
            return Fail(compressResult.Failures);
        }

        var compressedFileContent = compressResult.Value;

        var image = new Image
        {
            RecipeId = recipeResult.Value.Id,
            FileName = $"{Guid.NewGuid()}.webp",
        };

        await _data.Images.Add(image, cancellationToken);

        var blob = new Blob
        {
            Id = image.Id,
            Bytes = compressedFileContent,
        };

        await _data.Blobs.Add(blob, cancellationToken);

        return Ok(EntityMessage.Create("Image uploaded.", image.FileName));
    }

    private async Task<IResult<byte[]>> CompressImage(byte[] fileContent, int quality, int maxHeight, CancellationToken cancellationToken)
    {
        try
        {
            using var image = new MagickImage(fileContent);

            if (image.Format == MagickFormat.WebP)
            {
                _logger.LogInformation("Skipping compression. Already webp.");
                return Result.Ok(fileContent);
            }

            image.Strip();

            var defines = new WebPWriteDefines
            {
                Lossless = true,
                Method = 6,
            };

            if (quality < 100)
            {
                image.Quality = quality;
                defines.Lossless = false;
                defines.Method = 5;
            }

            if (image.Height > maxHeight)
            {
                image.Resize(0, maxHeight);
            }

            using var ms = new MemoryStream();
            await image.WriteAsync(ms, defines, cancellationToken);

            var compressedFileContent = ms.ToArray();

            _logger.LogInformation("Compressed image from {OldSize} KB to {NewSize} KB", fileContent.Length / 1024, compressedFileContent.Length / 1024);

            return Result.Ok(compressedFileContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception thrown while compressing image.");
            return Result.Fail<byte[]>(new Failure("Not a valid image.", "upload"));
        }
    }
}
