namespace FoodStuffs.Model.Data.Models;

public partial class Blob
{
    public int Id { get; set; }

    public byte[] Bytes { get; set; } = null!;

    public virtual Image Image { get; set; } = null!;
}
