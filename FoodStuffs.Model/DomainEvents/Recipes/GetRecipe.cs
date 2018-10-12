using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Logging;

namespace FoodStuffs.Model.DomainEvents.Recipes
{
    public class GetRecipe
    {
        public class Handler : EventHandlerAbstract<Request, RecipeDto>
        {
            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            protected override Result<RecipeDto> HandleInternal(Request request)
            {
                var dto = _data.Recipes.Stored
                    .WhereById(request.Id)
                    .Select(recipe => new RecipeDto(
                        recipe.Id,
                        recipe.Name,
                        recipe.Ingredients,
                        recipe.Directions,
                        recipe.CookTimeMinutes,
                        recipe.PrepTimeMinutes,
                        recipe.CreatedByUser.UserName,
                        recipe.CreatedOnUtc,
                        recipe.ModifiedByUser.UserName,
                        recipe.ModifiedOnUtc,
                        recipe.CategoryRecipe.Select(cr => cr.Category.Name)))
                    .FirstOrDefault();

                if (dto == null)
                {
                    return Result.Fail<RecipeDto>("Recipe not found.");
                }

                return Result.Ok(dto);
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
            public IEnumerable<string> Categories { get; } = new List<string>();
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
