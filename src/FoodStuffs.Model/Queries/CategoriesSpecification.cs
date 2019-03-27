using FoodStuffs.Model.Data.Models;
using System;
using System.Linq.Expressions;
using VoidCore.Model.Queries;

namespace FoodStuffs.Model.Queries
{
    public class CategoriesSpecification : QuerySpecificationAbstract<Category>
    {
        public CategoriesSpecification(params Expression<Func<Category, bool>>[] criteria) : base(criteria) { }
    }
}
