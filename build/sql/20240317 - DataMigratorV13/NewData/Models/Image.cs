using VoidCore.Model.Data;

namespace DataMigratorV13.NewData.Models;

public class Image : IAuditableWithOffset
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTimeOffset ModifiedOn { get; set; }

    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual ImageBlob ImageBlob { get; set; } = null!;
}
