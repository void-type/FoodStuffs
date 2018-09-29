using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FoodStuffs.Model.Services
{
    public class CategoryManager : ICategoryManager
    {
        public CategoryManager(IFoodStuffsData data)
        {
            _data = data;
        }

        public IEnumerable<Category> FindUnusedCategories(Recipe recipe, IEnumerable<CategoryRecipe> categoryRecipes)
        {
            var categoryIdsToCheck = categoryRecipes.Select(cr => cr.CategoryId);

            var categoryIdsToDelete = _data.CategoryRecipes.Stored
                .Where(cr => categoryIdsToCheck.Contains(cr.CategoryId))
                .GroupBy(cr => cr.CategoryId)
                .Where(group => group.All(cr => cr.RecipeId == recipe.Id))
                .Select(group => group.Key);

            return _data.Categories.Stored
                .Where(c => categoryIdsToDelete.Contains(c.Id));
        }

        public IEnumerable<string> CleanCategories(IEnumerable<string> categories)
        {
            return categories
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Trim()));
        }

        // Old ones
        private void AddCategoriesAndCategoryRecipes(int recipeId, IEnumerable<string> requestedCategories)
        {
            foreach (var viewModelCategoryName in requestedCategories)
            {
                var category = _data.Categories.Stored.GetByName(viewModelCategoryName) ?? CreateCategory(viewModelCategoryName);

                var existingCategoryRecipe = _data.CategoryRecipes.Stored.GetById(recipeId, category.Id);

                if (existingCategoryRecipe == null)
                {
                    CreateCategoryRecipe(recipeId, category);
                }
            }
        }

        private Category CreateCategory(string viewModelCategory)
        {
            var newCategory = _data.Categories.New;
            newCategory.Name = viewModelCategory;
            _data.Categories.Add(newCategory);
            return newCategory;
        }

        private void CreateCategoryRecipe(int recipeId, Category category)
        {
            var categoryRecipe = _data.CategoryRecipes.New;
            categoryRecipe.RecipeId = recipeId;
            categoryRecipe.CategoryId = category.Id;
            _data.CategoryRecipes.Add(categoryRecipe);
        }

        private IEnumerable<Category> FindUnusedCategories(Recipe recipe, IEnumerable<CategoryRecipe> unusedCategoryRecipes)
        {
            var categoryIds = unusedCategoryRecipes.Select(cr => cr.CategoryId);

            var categories = _data.Categories.Stored.Where(category => categoryIds.Contains(category.Id));

            foreach (var category in categories)
            {
                if (category.CategoryRecipe.All(cr => cr.RecipeId == recipe.Id))
                {
                    yield return category;
                }
            }
        }

        private IEnumerable<CategoryRecipe> FindUnusedCategoryRecipes(Recipe recipe, Request request)
        {
            var newCategoryNames = request.Categories.Select(c => c.ToUpper().Trim()).ToList();

            var unusedCategoryRecipes =
                recipe.CategoryRecipe.Where(cr => !newCategoryNames.Contains(cr.Category.Name.ToUpper().Trim()));

            return unusedCategoryRecipes;
        }

        private readonly IFoodStuffsData _data;
    }
}
