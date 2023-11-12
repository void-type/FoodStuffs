namespace FoodStuffs.Model.ImageCompression;

public sealed class ResizeSettings
{
    public ResizeOperation Operation { get; }
    public int MaxWidth { get; }
    public int MaxHeight { get; }
    public int RatioWidth { get; }
    public int RatioHeight { get; }

    private ResizeSettings(ResizeOperation operation, int maxWidth, int maxHeight, int ratioWidth, int ratioHeight)
    {
        Operation = operation;
        MaxWidth = maxWidth;
        MaxHeight = maxHeight;
        RatioWidth = ratioWidth;
        RatioHeight = ratioHeight;
    }

    /// <summary>
    /// Use the original dimensions.
    /// </summary>
    public static ResizeSettings None()
    {
        return new ResizeSettings(
            operation: ResizeOperation.None,
            maxWidth: 0,
            maxHeight: 0,
            ratioWidth: 0,
            ratioHeight: 0);
    }

    /// <summary>
    /// Take a crop starting from center to fit the aspect ratio given.
    /// </summary>
    /// <param name="ratioWidth">Width by ratio. Eg: 4</param>
    /// <param name="ratioHeight">Height by ratio. Eg: 3</param>
    /// <param name="maxWidth">Max width in pixels. Eg: 1600 (0 is unlimited)</param>
    public static ResizeSettings CenterCrop(int ratioWidth, int ratioHeight, int maxWidth)
    {
        return new ResizeSettings(
            operation: ResizeOperation.CenterCrop,
            maxWidth: maxWidth,
            maxHeight: 0,
            ratioWidth: ratioWidth,
            ratioHeight: ratioHeight);
    }

    /// <summary>
    /// Resize the image to fit in the given frame while respecting the original aspect ratio.
    /// </summary>
    /// <param name="maxWidth">Max width in pixels. Eg: 1600 (0 is unlimited)</param>
    /// <param name="maxHeight">Max height in pixels. Eg: 1200 (0 is unlimited)</param>
    public static ResizeSettings Fit(int maxWidth, int maxHeight)
    {
        return new ResizeSettings(
            operation: ResizeOperation.Fit,
            maxWidth: maxWidth,
            maxHeight: maxHeight,
            ratioWidth: 0,
            ratioHeight: 0);
    }
}
