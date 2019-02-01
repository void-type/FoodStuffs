using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;

namespace FoodStuffs.Model.Domain.Recipes
{
    public class GetRecipe
    {
        public class Handler : EventHandlerSyncAbstract<Request, RecipeDto>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            protected override IResult<RecipeDto> HandleSync(Request request)
            {
                return _data.Recipes.Stored
                    .GetById(request.Id)
                    .Select(recipe => new RecipeDto(
                        recipe.Id,
                        recipe.Name,
                        recipe.Ingredients,
                        recipe.Directions,
                        recipe.CookTimeMinutes,
                        recipe.PrepTimeMinutes,
                        recipe.CreatedBy,
                        recipe.CreatedOn,
                        recipe.ModifiedBy,
                        recipe.ModifiedOn,
                        recipe.CategoryRecipe.Select(cr => cr.Category.Name)))
                    .ToResult("Recipe not found.");
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

        public class RecipeDto
        {
            public RecipeDto(int id, string name, string ingredients, string directions, int? cookTimeMinutes, int? prepTimeMinutes,
                string createdBy, DateTime createdOnUtc, string modifiedBy, DateTime modifiedOnUtc, IEnumerable<string> categories)
            {
                Id = id;
                Name = name;
                Ingredients = ingredients;
                Directions = directions;
                CookTimeMinutes = cookTimeMinutes;
                PrepTimeMinutes = prepTimeMinutes;
                CreatedBy = createdBy;
                CreatedOnUtc = createdOnUtc;
                ModifiedBy = modifiedBy;
                ModifiedOnUtc = modifiedOnUtc;
                Categories = categories;
            }

            public int Id { get; }
            public string Name { get; }
            public string Ingredients { get; }
            public string Directions { get; }
            public int? CookTimeMinutes { get; }
            public int? PrepTimeMinutes { get; }
            public string CreatedBy { get; }
            public DateTime CreatedOnUtc { get; }
            public string ModifiedBy { get; }
            public DateTime ModifiedOnUtc { get; }
            public IEnumerable<string> Categories { get; }
        }

        public class Logger : FallibleEventLogger<Request, RecipeDto>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            public override void OnBoth(Request request, IResult<RecipeDto> result)
            {
                Logger.Info($"Id: '{request.Id}'");
                base.OnBoth(request, result);
            }
        }
    }
}
