﻿using FoodStuffs.Model.Data.Models;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealSetsSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsSpecification(Expression<Func<MealSet, bool>>[] criteria) : base(criteria)
    {
        AddOrderBy(m => m.CreatedOn, true);
    }
}
