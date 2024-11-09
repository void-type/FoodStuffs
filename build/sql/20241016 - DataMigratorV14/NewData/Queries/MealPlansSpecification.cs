using DataMigratorV14.NewData.Models;
using VoidCore.Model.Data;

namespace DataMigratorV14.NewData.Queries;

public class MealPlansSpecification : QuerySpecificationAbstract<MealPlan>
{
    public MealPlansSpecification()
    {
        AddOrderBy(m => m.CreatedOn, true);
    }
}
