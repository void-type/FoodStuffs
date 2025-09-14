using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Events.Images.Models;
using FoodStuffs.Model.ImageCompression;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class SaveImageHandler : CustomEventHandlerAbstract<SaveImageRequest, EntityMessage<string>>
{
    private readonly FoodStuffsContext _data;
    private readonly ILogger<SaveImageHandler> _logger;
    private readonly IImageCompressionService _compressor;
    private readonly ISearchIndexService _searchIndex;

    public SaveImageHandler(FoodStuffsContext data, ILogger<SaveImageHandler> logger, IImageCompressionService imageCompressionService, ISearchIndexService searchIndex)
    {
        _data = data;
        _logger = logger;
        _compressor = imageCompressionService;
        _searchIndex = searchIndex;
    }

    public override async Task<IResult<EntityMessage<string>>> Handle(SaveImageRequest request, CancellationToken cancellationToken = default)
    {
        // If we want to handle large files via streaming, we can use https://code-maze.com/aspnetcore-upload-large-files/.
        // IFormFile is still buffered by model binding.

        // Note: Size limit is controlled by the server (IIS and/or Kestrel) and validated on the client. Default is 30MB (~28.6 MiB).
        // To change this, you will need both:
        // 1. a web.config with requestLimits maxAllowedContentLength="<byte size>"
        // 2. configure FormOptions in startup for options.MultipartBodyLengthLimit = <byte size>
        // 3. edit the client-side upload validation in the RecipeEdit.vue file.

        var recipeResult = await _data.Recipes
            .TagWith(GetTag())
            .AsSingleQuery()
            .Include(x => x.Images)
            .FirstOrDefaultAsync(r => r.Id == request.RecipeId, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new RecipeNotFoundFailure());

        if (recipeResult.IsFailed)
        {
            return Fail(recipeResult.Failures);
        }

        var compressResult = await CompressImageAsync(request, cancellationToken);

        if (compressResult.IsFailed)
        {
            return Fail(compressResult.Failures);
        }

        var compressedFileContent = compressResult.Value;

        var image = new Image
        {
            FileName = $"{Guid.NewGuid()}.webp",
            ImageBlob = new ImageBlob
            {
                Bytes = compressedFileContent,
            }
        };

        recipeResult.Value.Images.Add(image);

        await _data.SaveChangesAsync(cancellationToken);

        await _searchIndex.AddOrUpdateAsync(SearchIndex.Recipes, recipeResult.Value.Id, cancellationToken);

        return Ok(EntityMessage.Create("Image uploaded.", image.FileName));
    }

    private async Task<IResult<byte[]>> CompressImageAsync(SaveImageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await using var compressedFileContent = await _compressor
                .CompressAndResizeImageAsync(request.FileStream, 95, ResizeSettings.CenterCrop(4, 3, 1600), cancellationToken);

            return Result.Ok(compressedFileContent.ToArray());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception thrown while compressing image.");
            return Result.Fail<byte[]>(new Failure("There was an error while processing your image.", "upload-file"));
        }
    }
}
