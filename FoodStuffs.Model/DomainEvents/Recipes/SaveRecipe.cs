using AutoMapper;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.Time;
using VoidCore.Model.Validation;

namespace FoodStuffs.Model.DomainEvents.Recipes
{
    public class SaveRecipe
    {
        public class Handler : EventHandlerAbstract<Request, PostSuccessUserMessage<int>>
        {
            public Handler(IFoodStuffsData data, IMapper mapper, IDateTimeService now, ICurrentUserAccessor userAccessor)
            {
                _mapper = mapper;
                _data = data;
                _now = now.Moment;
                _user = userAccessor.User;
            }

            protected override Result<PostSuccessUserMessage<int>> HandleInternal(Request request)
            {
                var recipe = _data.Recipes.Stored
                    .WhereById(request.Id)
                    .FirstOrDefault() ?? NewFromData();

                _mapper.Map(request, recipe);
                recipe.ModifiedOnUtc = _now;
                recipe.ModifiedByUserId = _user.Id;

                var cleanedRequestedCategories = CleanCategoryNames(request.Categories);

                var currentCategoriesAndCategoryRecipes = _data.CategoryRecipes.Stored
                    .WhereForRecipe(recipe.Id)
                    .Join(_data.Categories.Stored,
                        cr => cr.CategoryId,
                        c => c.Id,
                        (cr, c) => new { Category = c, CategoryRecipe = cr });

                var categoriesToCreate = cleanedRequestedCategories
                    .Where(cName => !_data.Categories.Stored
                        .Select(c => c.Name)
                        .Contains(cName))
                    .Select(cName => CreateCategory(cName));

                _data.Categories.AddRange(categoriesToCreate);

                var categoryRecipesToRemove = currentCategoriesAndCategoryRecipes
                    .Where(crc => !cleanedRequestedCategories.Contains(crc.Category.Name))
                    .Select(crc => crc.CategoryRecipe);

                _data.CategoryRecipes.RemoveRange(categoryRecipesToRemove);

                var categoryRecipesToCreate = _data.Categories.Stored
                    .Where(c => cleanedRequestedCategories.Contains(c.Name))
                    .Where(c => !currentCategoriesAndCategoryRecipes
                        .Select(crc => crc.Category.Name)
                        .Contains(c.Name))
                    .Select(c => CreateCategoryRecipe(recipe.Id, c));

                _data.CategoryRecipes.AddRange(categoryRecipesToCreate);

                _data.SaveChanges();

                return Result.Ok(PostSuccessUserMessage.Create<int>("Recipe saved.", request.Id));
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

            private IEnumerable<string> CleanCategoryNames(IEnumerable<string> categories)
            {
                return categories
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Select(c => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Trim()));
            }

            private Recipe NewFromData()
            {
                var recipe = _data.Recipes.New;
                recipe.CreatedOnUtc = _now;
                recipe.CreatedByUserId = _user.Id;
                return recipe;
            }

            private readonly IFoodStuffsData _data;
            private readonly IMapper _mapper;
            private readonly DateTime _now;
            private readonly User _user;
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

        public class RequestValidator : ValidatorAbstract<Request>
        {
            protected override void BuildRules()
            {
                CreateRule("Please enter a name.", "name")
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

                CreateRule("Please enter ingredients.", "ingredients")
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Ingredients));

                CreateRule("Please enter directions.", "directions")
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Directions));

                CreateRule("Cook time must be positive.", "cookTimeMinutes")
                    .InvalidWhen(entity => entity.CookTimeMinutes < 0)
                    .ExceptWhen(entity => entity.CookTimeMinutes == null);

                CreateRule("Prep time must be positive.", "prepTimeMinutes")
                    .InvalidWhen(entity => entity.PrepTimeMinutes < 0)
                    .ExceptWhen(entity => entity.PrepTimeMinutes == null);
            }
        }

        public class Logger : PostSuccessUserMessageEventLogger<Request, int>
        {
            public Logger(ILoggingService logger) : base(logger) { }
        }
    }
}
