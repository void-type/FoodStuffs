using FoodStuffs.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.Actions.Responder;
using VoidCore.Model.Actions.Steps;

namespace FoodStuffs.Model.Actions.Recipes
{
    public class SortRecipes : AbstractActionStep
    {
        public SortRecipes(string sort, List<IRecipeViewModel> context)
        {
            _sort = sort;
            _context = context;
        }

        protected override void PerformStep(IActionResponder respond)
        {
            var temp = _context.ToArray();
            _context.Clear();

            switch (_sort)
            {
                case "ascending":
                    _context.AddRange(temp.OrderBy(recipe => recipe.Name));
                    break;

                case "descending":
                    _context.AddRange(temp.OrderByDescending(recipe => recipe.Name));
                    break;

                default:
                    _context.AddRange(temp.OrderBy(recipe => recipe.Id));
                    break;
            }
        }

        private readonly List<IRecipeViewModel> _context;
        private readonly string _sort;
    }
}
