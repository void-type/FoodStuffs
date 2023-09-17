using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class ImagesByNameWithBlobsSpecification : QuerySpecificationAbstract<Image>
{
    public ImagesByNameWithBlobsSpecification(string name)
    {
        AddCriteria(i => i.FileName == name);
        AddInclude(nameof(Image.Blob));
    }
}
