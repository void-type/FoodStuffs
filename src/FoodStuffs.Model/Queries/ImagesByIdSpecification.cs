using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Queries
{
    public class ImagesByIdSpecification : QuerySpecificationAbstract<Image>
    {
        public ImagesByIdSpecification(int id) : base()
        {
            AddCriteria(i => i.Id == id);
        }
    }
}
