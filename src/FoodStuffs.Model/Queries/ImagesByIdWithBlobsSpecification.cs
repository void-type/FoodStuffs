using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Queries
{
    public class ImagesByIdWithBlobsSpecification : QuerySpecificationAbstract<Image>
    {
        public ImagesByIdWithBlobsSpecification(int id) : base()
        {
            AddCriteria(i => i.Id == id);
            AddInclude($"{nameof(Image.Blob)}");
        }
    }
}
