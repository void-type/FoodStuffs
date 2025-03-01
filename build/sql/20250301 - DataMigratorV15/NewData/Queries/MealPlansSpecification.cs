using DataMigratorV15.NewData.Models;
using VoidCore.Model.Data;

namespace DataMigratorV15.NewData.Queries;

public class MealPlansSpecification : QuerySpecificationAbstract<MealPlan>
{
    public MealPlansSpecification()
    {
        AddOrderBy(m => m.CreatedOn, true);
    }
}
