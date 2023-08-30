using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ImagesByIdWithRecipesSpecification : QuerySpecificationAbstract<Image>
{
    public ImagesByIdWithRecipesSpecification(int id)
    {
        AddCriteria(i => i.Id == id);
        AddInclude(nameof(Image.Recipe));
    }
}
