using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ImagesByNameWithRecipesSpecification : QuerySpecificationAbstract<Image>
{
    public ImagesByNameWithRecipesSpecification(string name)
    {
        AddCriteria(i => i.FileName == name);
        AddInclude(nameof(Image.Recipe));
    }
}
