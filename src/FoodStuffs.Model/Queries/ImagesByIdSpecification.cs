using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Queries
{
    public class ImagesByIdSpecification : QuerySpecificationAbstract<Image>
    {
        public ImagesByIdSpecification(int id) : base(r => r.Id == id)
        {
        }
    }
}
