namespace DataMigratorV13.OldData.Models;

public partial class Image
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public int RecipeId { get; set; }

    public virtual Blob? Blob { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
