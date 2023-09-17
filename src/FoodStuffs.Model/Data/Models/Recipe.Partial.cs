using VoidCore.Model.Data;

#nullable disable

namespace FoodStuffs.Model.Data.Models;

public partial class Recipe : IAuditable
{
    public Image DefaultImage => PinnedImage ?? Images.FirstOrDefault();
}
