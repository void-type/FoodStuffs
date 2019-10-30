using System;
using System.Linq.Expressions;
using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Queries
{
    public class CategoriesSpecification : QuerySpecificationAbstract<Category>
    {
        public CategoriesSpecification(params Expression<Func<Category, bool>>[] criteria) : base(criteria) { }
    }
}
