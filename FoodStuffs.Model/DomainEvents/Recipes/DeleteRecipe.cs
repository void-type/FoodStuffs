using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Message;

namespace FoodStuffs.Model.DomainEvents.Recipes
{
    public class DeleteRecipe
    {
        public class Handler : DomainEventAbstract<Request, PostSuccessUserMessage<int>>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            protected override Result<PostSuccessUserMessage<int>> HandleInternal(Request request)
            {
                var recipe = _data.Recipes.Stored
                    .WhereById(request.Id)
                    .FirstOrDefault();

                if (recipe == null)
                {
                    return Result.Fail<PostSuccessUserMessage<int>>("Recipe not found.");
                }

                var recipeCategories = recipe.CategoryRecipe;
                _data.CategoryRecipes.RemoveRange(recipeCategories);

                var unusedCategories = FindUnusedCategories(recipe);
                _data.Categories.RemoveRange(unusedCategories);

                _data.Recipes.Remove(recipe);

                _data.SaveChanges();

                return Result.Ok(PostSuccessUserMessage.Create<int>("Recipe deleted.", request.Id));
            }

            private IEnumerable<Category> FindUnusedCategories(Recipe recipe)
            {
                var categoryIdsToCheck = recipe.CategoryRecipe.Select(cr => cr.CategoryId);

                var categoriesToCheck = _data.Categories.Stored.Where(c => categoryIdsToCheck.Contains(c.Id));

                foreach (var categoryToCheck in categoriesToCheck)
                {
                    if (_data.CategoryRecipes.Stored.Where(cr => cr.CategoryId == categoryToCheck.Id)
                        .All(cr => cr.RecipeId == recipe.Id))
                    {
                        yield return categoryToCheck;
                    }
                }
            }

            private readonly IFoodStuffsData _data;
        }

        public class Request
        {
            public Request(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Logging : PostSuccessUserMessageLogging<Request, int>
        {
            public Logging(ILoggingService logger) : base(logger) { }

            public override void OnBoth(Request request, IResult<PostSuccessUserMessage<int>> result)
            {
                Logger.Info($"Id: '{request.Id}'");
                base.OnBoth(request, result);
            }
        }
    }
}
