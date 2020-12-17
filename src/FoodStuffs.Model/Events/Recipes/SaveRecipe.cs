using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Queries;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Domain;
using VoidCore.Domain.Events;
using VoidCore.Domain.RuleValidator;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Messages;

// Allow single file events
#pragma warning disable CA1034

namespace FoodStuffs.Model.Events.Recipes
{
    public static class SaveRecipe
    {
        public class Handler : EventHandlerAbstract<Request, EntityMessage<int>>
        {
            private readonly IFoodStuffsData _data;

            public Handler(IFoodStuffsData data)
            {
                _data = data;
            }

            public override async Task<IResult<EntityMessage<int>>> Handle(Request request, CancellationToken cancellationToken = default)
            {
                var byId = new RecipesByIdWithCategoriesAndImagesSpecification(request.Id);

                var maybeRecipe = await _data.Recipes.Get(byId, cancellationToken)
                    .ConfigureAwait(false);

                if (maybeRecipe.HasValue)
                {
                    return await maybeRecipe.Value
                        .Tee(r => Transfer(request, r))
                        .TeeAsync(r => _data.Recipes.Update(r, cancellationToken))
                        .TeeAsync(r => ManageCategories(request, r, cancellationToken))
                        .MapAsync(r => Ok(EntityMessage.Create("Recipe updated.", r.Id)))
                        .ConfigureAwait(false);
                }

                return await new Recipe()
                    .Tee(r => Transfer(request, r))
                    .TeeAsync(r => _data.Recipes.Add(r, cancellationToken))
                    .TeeAsync(r => ManageCategories(request, r, cancellationToken))
                    .MapAsync(r => Ok(EntityMessage.Create("Recipe added.", r.Id)))
                    .ConfigureAwait(false);
            }

            private static void Transfer(Request request, Recipe recipe)
            {
                recipe.Name = request.Name;
                recipe.Ingredients = request.Ingredients;
                recipe.Directions = request.Directions;
                recipe.CookTimeMinutes = request.CookTimeMinutes;
                recipe.PrepTimeMinutes = request.PrepTimeMinutes;
            }

            private async Task ManageCategories(Request request, Recipe recipe, CancellationToken cancellationToken)
            {
                var requested = request.Categories
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Select(n => n.ToLower().Trim())
                    .ToArray();

                var categoriesThatMatchRequestedSpec = new CategoriesSpecification(
                    c => requested.Contains(c.Name.ToLower().Trim()));

                var categoriesExist = (await _data.Categories
                    .List(categoriesThatMatchRequestedSpec, cancellationToken)
                    .ConfigureAwait(false))
                    .Select(c => c.Name.ToLower().Trim());

                // Add categories that don't exist
                await requested
                    .Where(n => !categoriesExist.Contains(n))
                    .Select(n => new Category { Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(n) })
                    .TeeAsync(r => _data.Categories.AddRange(r, cancellationToken))
                    .ConfigureAwait(false);

                // Remove relations that are no longer needed
                await recipe.CategoryRecipes
                    .Where(r => !requested.Contains(r.Category.Name.ToLower().Trim()))
                    .TeeAsync(r => _data.CategoryRecipes.RemoveRange(r, cancellationToken))
                    .ConfigureAwait(false);

                // Add relations that don't exist
                await _data.Categories
                    .List(categoriesThatMatchRequestedSpec, cancellationToken)
                    .MapAsync(categories => categories
                       .Where(c => !recipe.CategoryRecipes
                          .Select(r => r.Category.Name.ToLower().Trim())
                          .Contains(c.Name.ToLower().Trim()))
                       .Select(c => new CategoryRecipe
                       {
                           RecipeId = recipe.Id,
                           CategoryId = c.Id
                       }))
                    .TeeAsync(r => _data.CategoryRecipes.AddRange(r, cancellationToken))
                    .ConfigureAwait(false);
            }
        }

        public record Request(
            int Id,
            string Name,
            string Ingredients,
            string Directions,
            int? CookTimeMinutes,
            int? PrepTimeMinutes,
            IEnumerable<string> Categories);

        public class RequestValidator : RuleValidatorAbstract<Request>
        {
            public RequestValidator()
            {
                CreateRule(new Failure("Please enter a name.", "name"))
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Name));

                CreateRule(new Failure("Please enter ingredients.", "ingredients"))
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Ingredients));

                CreateRule(new Failure("Please enter directions.", "directions"))
                    .InvalidWhen(entity => string.IsNullOrWhiteSpace(entity.Directions));

                CreateRule(new Failure("Cook time must be positive.", "cookTimeMinutes"))
                    .InvalidWhen(entity => entity.CookTimeMinutes < 0);

                CreateRule(new Failure("Prep time must be positive.", "prepTimeMinutes"))
                    .InvalidWhen(entity => entity.PrepTimeMinutes < 0);
            }
        }

        public class Logger : EntityMessageEventLogger<Request, int>
        {
            public Logger(ILoggingService logger) : base(logger) { }

            protected override void OnBoth(Request request, IResult<EntityMessage<int>> result)
            {
                Logger.Info($"RequestRecipeId: {request.Id}");
                base.OnBoth(request, result);
            }
        }
    }
}
