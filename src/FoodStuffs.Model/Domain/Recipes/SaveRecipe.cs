using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Domain.RuleValidator;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Domain.Recipes
{
    public class SaveRecipe
    {
        public class Handler : EventHandlerSyncAbstract<Request, UserMessageWithEntityId<int>>
        {
            public Handler(IFoodStuffsData data, IAuditUpdater auditUpdater)
            {
                _data = data;
                _auditUpdater = auditUpdater;
            }

            protected override IResult<UserMessageWithEntityId<int>> HandleSync(Request request)
            {
                return _data.Recipes.Stored
                    .GetById(request.Id)
                    .Unwrap(CreateNewRecipe)
                    .Tee(r => UpdateRecipe(r, request))
                    .Tee(r => ManageCategories(r, request))
                    .Tee(r => _data.SaveChanges())
                    .Map(r => Result.Ok(UserMessageWithEntityId.Create("Recipe saved.", r.Id)));
            }

            private Recipe CreateNewRecipe()
            {
                var recipe = _data.Recipes.New;
                _data.Recipes.Add(recipe);
                _auditUpdater.Create(recipe);
                return recipe;
            }

            private void UpdateRecipe(Recipe recipe, Request request)
            {
                recipe.Name = request.Name;
                recipe.Ingredients = request.Ingredients;
                recipe.Directions = request.Directions;
                recipe.CookTimeMinutes = request.CookTimeMinutes;
                recipe.PrepTimeMinutes = request.PrepTimeMinutes;
                _auditUpdater.Update(recipe);
            }

            private void ManageCategories(Recipe recipe, Request request)
            {
                var requested = request.Categories
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Select(n => n.ToLower().Trim())
                    .ToArray();

                var categoriesToCreate = requested
                    .Where(n => !_data.Categories.Stored
                        .Select(c => c.Name.ToLower())
                        .Contains(n))
                    .Select(n => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(n))
                    .Select(CreateCategory);

                _data.Categories.AddRange(categoriesToCreate);

                _data.SaveChanges();

                var currentRelations = _data.CategoryRecipes.Stored
                    .Where(cr => cr.RecipeId == recipe.Id);

                var relationsToRemove = currentRelations
                    .Where(r => !requested.Contains(r.Category.Name.ToLower()));

                _data.CategoryRecipes.RemoveRange(relationsToRemove);

                var relationsToCreate = _data.Categories.Stored
                    .Where(c => requested.Contains(c.Name.ToLower()))
                    .Where(c => !currentRelations
                        .Select(r => r.Category.Name.ToLower())
                        .Contains(c.Name))
                    .Select(c => CreateCategoryRecipe(recipe.Id, c));

                _data.CategoryRecipes.AddRange(relationsToCreate);
            }

            private CategoryRecipe CreateCategoryRecipe(int recipeId, Category category)
            {
                var categoryRecipe = _data.CategoryRecipes.New;
                categoryRecipe.RecipeId = recipeId;
                categoryRecipe.CategoryId = category.Id;
                return categoryRecipe;
            }

            private Category CreateCategory(string viewModelCategory)
            {
                var category = _data.Categories.New;
                category.Name = viewModelCategory;
                return category;
            }

            private readonly IFoodStuffsData _data;
            private readonly IAuditUpdater _auditUpdater;
        }

        public class Request
        {
            public Request(int id, string name, string ingredients, string directions, int? cookTimeMinutes, int? prepTimeMinutes, IEnumerable<string> categories)
            {
                Id = id;
                Name = name;
                Ingredients = ingredients;
                Directions = directions;
                CookTimeMinutes = cookTimeMinutes;
                PrepTimeMinutes = prepTimeMinutes;
                Categories = categories;
            }

            public int Id { get; }
            public string Name { get; }
            public string Ingredients { get; }
            public string Directions { get; }
            public int? CookTimeMinutes { get; }
            public int? PrepTimeMinutes { get; }
            public IEnumerable<string> Categories { get; }
        }

        public class RequestValidator : RuleValidatorAbstract<Request>
        {
            public RequestValidator()
            {
                CreateRule("Please enter a name.", "name")
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

                CreateRule("Please enter ingredients.", "ingredients")
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Ingredients));

                CreateRule("Please enter directions.", "directions")
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Directions));

                CreateRule("Cook time must be positive.", "cookTimeMinutes")
                    .InvalidWhen(entity => entity.CookTimeMinutes < 0);

                CreateRule("Prep time must be positive.", "prepTimeMinutes")
                    .InvalidWhen(entity => entity.PrepTimeMinutes < 0);
            }
        }

        public class Logger : UserMessageWithEntityIdEventLogger<Request, int>
        {
            public Logger(ILoggingService logger) : base(logger) { }
        }
    }
}
