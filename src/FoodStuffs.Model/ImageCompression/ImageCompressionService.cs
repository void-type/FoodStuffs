using ImageMagick;
using ImageMagick.Formats;
using Microsoft.Extensions.Logging;

namespace FoodStuffs.Model.ImageCompression;

public class ImageCompressionService : IImageCompressionService
{
    private readonly ILogger<ImageCompressionService> _logger;

    public ImageCompressionService(ILogger<ImageCompressionService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<MemoryStream> CompressAndResizeImage(Stream fileStream, int quality, ResizeSettings resizeSettings, CancellationToken cancellationToken)
    {
        using var image = new MagickImage(fileStream);

        // Preserve orientation since we'll strip Exif data.
        image.AutoOrient();
        image.Strip();

        if (image.Format == MagickFormat.WebP)
        {
            _logger.LogInformation("Skipping compression. Already webp.");

            var uncompressedStream = new MemoryStream();
            await image.WriteAsync(uncompressedStream, cancellationToken);
            return uncompressedStream;
        }

        var defines = new WebPWriteDefines
        {
            Lossless = true,
            Method = 6,
        };

        if (quality < 100)
        {
            image.Quality = (uint)quality;
            defines.Lossless = false;
            defines.Method = 5;
        }

        switch (resizeSettings.Operation)
        {
            case ResizeOperation.CenterCrop:
                image.Crop(new MagickGeometry($"{resizeSettings.RatioWidth}:{resizeSettings.RatioHeight}"), Gravity.Center);

                if (ShouldResizeWidth(resizeSettings, image))
                {
                    image.Resize((uint)resizeSettings.MaxWidth, 0);
                }
                break;

            case ResizeOperation.Fit:
                if (ShouldResizeWidth(resizeSettings, image) || ShouldResizeHeight(resizeSettings, image))
                {
                    image.Resize((uint)resizeSettings.MaxWidth, (uint)resizeSettings.MaxHeight);
                }
                break;
        }

        var outputStream = new MemoryStream();
        await image.WriteAsync(outputStream, defines, cancellationToken);

        _logger.LogInformation("Compressed image from {OldSize} KB to {NewSize} KB.", fileStream.Length / 1024, outputStream.Length / 1024);

        return outputStream;
    }

    private static bool ShouldResizeWidth(ResizeSettings resizeSettings, MagickImage image)
    {
        return resizeSettings.MaxWidth > 0 && image.Width > resizeSettings.MaxWidth;
    }

    private static bool ShouldResizeHeight(ResizeSettings resizeSettings, MagickImage image)
    {
        return resizeSettings.MaxHeight > 0 && image.Height > resizeSettings.MaxHeight;
    }
}
