using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Queries
{
    public class ImagesByIdWithBlobsSpecification : QuerySpecificationAbstract<Image>
    {
        public ImagesByIdWithBlobsSpecification(int id) : base(r => r.Id == id)
        {
            AddInclude($"{nameof(Image.Blob)}");
        }
    }
}
