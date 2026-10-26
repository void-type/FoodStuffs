namespace FoodStuffs.Model.RecycleBin;

public class RecycleBinSettings
{
    /// <summary>
    /// Days a recipe stays in the recycle bin before it is permanently deleted automatically. Set to 0 to keep recipes until they are deleted by hand.
    /// </summary>
    public int RetentionDays { get; init; } = 90;

    public bool IsAutoPurgeEnabled => RetentionDays > 0;
}
