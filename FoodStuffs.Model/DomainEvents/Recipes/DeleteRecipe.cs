using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using FoodStuffs.Model.Services;
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
            public Handler(IFoodStuffsData data, ICategoryManager categoryManager)
            {
                _data = data;
                _categoryManager = categoryManager;
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

                var categoryRecipes = _data.CategoryRecipes.Stored.Where(cr => cr.RecipeId == recipe.Id);
                var unusedCategories = _categoryManager.FindUnusedCategories(recipe, categoryRecipes);

                _data.CategoryRecipes.RemoveRange(categoryRecipes);
                _data.Categories.RemoveRange(unusedCategories);
                _data.Recipes.Remove(recipe);

                _data.SaveChanges();

                return Result.Ok(PostSuccessUserMessage.Create<int>("Recipe deleted.", request.Id));
            }

            private readonly IFoodStuffsData _data;
            private readonly ICategoryManager _categoryManager;
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
