using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Queries;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Model.Logging;

namespace FoodStuffs.Model.Events.Recipes
{
    public class GetRecipe
    {
        public class Handler : EventHandlerAbstract<Request, RecipeDto>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<RecipeDto>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new RecipesByIdWithCategoriesAndImagesSpecification(request.Id);

                return await _data.Recipes.Get(byId, cancellationToken)
                    .ToResultAsync(new RecipeNotFoundFailure())
                    .SelectAsync(r => new RecipeDto(
                       id: r.Id,
                       name: r.Name,
                       ingredients: r.Ingredients,
                       directions: r.Directions,
                       cookTimeMinutes: r.CookTimeMinutes,
                       prepTimeMinutes: r.PrepTimeMinutes,
                       createdBy: r.CreatedBy,
                       createdOn: r.CreatedOn,
                       modifiedBy: r.ModifiedBy,
                       modifiedOn: r.ModifiedOn,
                       categories: r.CategoryRecipe.Select(cr => cr.Category.Name).OrderBy(n => n),
                       images: r.Image.Select(i => i.Id)));
            }
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
                string createdBy, DateTime createdOn, string modifiedBy, DateTime modifiedOn, IEnumerable<string> categories, IEnumerable<int> images)
            {
                Id = id;
                Name = name;
                Ingredients = ingredients;
                Directions = directions;
                CookTimeMinutes = cookTimeMinutes;
                PrepTimeMinutes = prepTimeMinutes;
                CreatedBy = createdBy;
                CreatedOn = createdOn;
                ModifiedBy = modifiedBy;
                ModifiedOn = modifiedOn;
                Categories = categories;
                Images = images;
            }

            public int Id { get; }
            public string Name { get; }
            public string Ingredients { get; }
            public string Directions { get; }
            public int? CookTimeMinutes { get; }
            public int? PrepTimeMinutes { get; }
            public string CreatedBy { get; }
            public DateTime CreatedOn { get; }
            public string ModifiedBy { get; }
            public DateTime ModifiedOn { get; }
            public IEnumerable<string> Categories { get; }
            public IEnumerable<int> Images { get; }
        }

        public class Logger : FallibleEventLogger<Request, RecipeDto>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<RecipeDto> result)
            {
                Logger.Info($"RequestRecipeId: {request.Id}");
                base.OnBoth(request, result);
            }
        }
    }
}
