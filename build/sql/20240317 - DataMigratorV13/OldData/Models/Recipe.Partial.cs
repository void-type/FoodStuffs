using VoidCore.Model.Data;

#nullable disable

namespace DataMigratorV13.OldData.Models;

public partial class Recipe : IAuditable
{
    public Image DefaultImage => PinnedImage ?? Images.FirstOrDefault();
}
