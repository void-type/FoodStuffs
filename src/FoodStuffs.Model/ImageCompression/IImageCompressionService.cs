namespace FoodStuffs.Model.ImageCompression;

public interface IImageCompressionService
{
    /// <summary>
    /// Compress an image to webp from most formats to optimize for web usage.
    /// Quality is positive integer up to 100. 95 is a good near-lossless value to start with. 75 is the default of the most libraries but results in a visual drop in quality.
    /// If you will be adding a blur or darkening effect over the image in CSS, say for a full-screen hero, then you can greatly lower the quality (70 might be a good starting point).
    /// The resize operation should be used to limit final output size or optionally crop the image aspect ratio.
    /// Size estimates using quality of 95:
    /// - full-width on desktop (2500px, 4:3) = 700kb
    /// - CTA size on desktop, 1/2 container width (1600px, 4:3) = 300kb
    /// - full-width on mobile (800px, 4:3) = 100kb
    /// - product thumbnail on mobile (250px, 1:1) = 20kb
    /// </summary>
    /// <param name="fileStream">A stream of the image data</param>
    /// <param name="quality">Quality value</param>
    /// <param name="resizeSettings">Resize operation settings</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<MemoryStream> CompressAndResizeImageAsync(Stream fileStream, int quality, ResizeSettings resizeSettings, CancellationToken cancellationToken);
}
