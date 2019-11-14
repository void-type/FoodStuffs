using System.Linq;
using System.Threading.Tasks;
using FoodStuffs.Model.Events;
using FoodStuffs.Model.Events.Images;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Responses.Files;
using Xunit;

namespace FoodStuffs.Test.Model.Events
{
    public class ImageEventTests
    {
        [Fact]
        public async Task GetImage_gets_image_by_id_if_exists()
        {
            using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var image = context.Image
                .Include(a => a.Blob)
                .First();

            var request = new GetImage.Request(image.Id);

            var result = await new GetImage.Handler(data).Handle(request);

            Assert.True(result.IsSuccess);

            var file = result.Value;

            Assert.Equal(image.Id.ToString(), file.Name);
            Assert.Equal("seeded file content", file.Content.ToString());
        }

        [Fact]
        public async Task GetImage_fails_if_image_not_found()
        {
            using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var request = new GetImage.Request(-5);

            var result = await new GetImage.Handler(data).Handle(request);

            Assert.True(result.IsFailed);
            Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
        }

        [Fact]
        public async Task SaveImage_creates_an_image_and_blob()
        {
            using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var recipe = context.Recipe.First(r => r.Name == "Recipe2");

            var myFile = new SimpleFile("my file content", "myFile.txt");

            var request = new SaveImage.Request(recipe.Id, myFile.Content.AsBytes);

            var result = await new SaveImage.Handler(data).Handle(request);

            Assert.True(result.IsSuccess);

            var image = context.Image.Include(a => a.Blob).First(a => a.Id == result.Value.Id);

            Assert.Equal(myFile.Content.AsBytes, image.Blob.Bytes);
            Assert.Equal(recipe.Id, image.RecipeId);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, image.ModifiedOn);
        }

        [Fact]
        public async Task SaveImage_fails_if_recipe_not_found()
        {
            using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var myFile = new SimpleFile("my file content", "myFile.txt");

            var request = new SaveImage.Request(-5, myFile.Content.AsBytes);

            var result = await new SaveImage.Handler(data).Handle(request);

            Assert.True(result.IsFailed);
            Assert.Contains(typeof(RecipeNotFoundFailure), result.Failures.Select(f => f.GetType()));
        }

        [Fact]
        public async Task DeleteImage_deletes_the_image_and_blob()
        {
            // Due to the way we delete, we need a fresh dbcontext to remove tracked entities.
            using var context1 = Deps.FoodStuffsContext("delete images success").Seed();

            using var context = Deps.FoodStuffsContext("delete images success");
            var data = context.FoodStuffsData();

            var recipe = context.Recipe
                .Include(r => r.Image)
                .ThenInclude(r => r.Blob)
                .AsNoTracking()
                .First(r => r.Name == "Recipe1");

            var image = recipe.Image.First();

            var request = new DeleteImage.Request(image.Id);

            var result = await new DeleteImage.Handler(data).Handle(request);

            Assert.True(result.IsSuccess);

            Assert.Empty(context.Image.Where(a => a.Id == image.Id).AsNoTracking().ToList());
            Assert.Empty(context.Blob.Where(b => b.Id == image.Blob.Id).AsNoTracking().ToList());
        }

        [Fact]
        public async Task DeleteImage_fails_if_recipe_not_found()
        {
            using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var request = new DeleteImage.Request(-5);

            var result = await new DeleteImage.Handler(data).Handle(request);

            Assert.True(result.IsFailed);
            Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
        }
    }
}
