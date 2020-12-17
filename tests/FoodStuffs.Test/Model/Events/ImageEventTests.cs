using FoodStuffs.Model.Events;
using FoodStuffs.Model.Events.Images;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VoidCore.Model.Responses.Files;
using Xunit;

namespace FoodStuffs.Test.Model.Events
{
    public class ImageEventTests
    {
        [Fact]
        public async Task GetImage_gets_image_by_id_if_exists()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var image = context.Images
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
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var request = new GetImage.Request(-5);

            var result = await new GetImage.Handler(data).Handle(request);

            Assert.True(result.IsFailed);
            Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
        }

        [Fact]
        public async Task SaveImage_creates_an_image_and_blob()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var recipe = context.Recipes.First(r => r.Name == "Recipe2");

            var myFile = new SimpleFile("my file content", "myFile.txt");

            var request = new SaveImage.Request(recipe.Id, myFile.Content.AsBytes);

            var result = await new SaveImage.Handler(data).Handle(request);

            Assert.True(result.IsSuccess);

            var image = context.Images.Include(a => a.Blob).First(a => a.Id == result.Value.Id);

            Assert.Equal(myFile.Content.AsBytes, image.Blob.Bytes);
            Assert.Equal(recipe.Id, image.RecipeId);
            Assert.Equal(Deps.DateTimeServiceLate.Moment, image.ModifiedOn);
        }

        [Fact]
        public async Task SaveImage_fails_if_recipe_not_found()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
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
            await using var context1 = Deps.FoodStuffsContext("delete images success").Seed();

            await using var context = Deps.FoodStuffsContext("delete images success");
            var data = context.FoodStuffsData();

            var recipe = context.Recipes
                .Include(r => r.Images)
                .ThenInclude(r => r.Blob)
                .AsNoTracking()
                .First(r => r.Name == "Recipe1");

            var image = recipe.Images.First();

            var request = new DeleteImage.Request(image.Id);

            var result = await new DeleteImage.Handler(data).Handle(request);

            Assert.True(result.IsSuccess);

            Assert.Empty(context.Images.Where(a => a.Id == image.Id).AsNoTracking().ToList());
            Assert.Empty(context.Blobs.Where(b => b.Id == image.Blob.Id).AsNoTracking().ToList());
        }

        [Fact]
        public async Task DeleteImage_fails_if_recipe_not_found()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var request = new DeleteImage.Request(-5);

            var result = await new DeleteImage.Handler(data).Handle(request);

            Assert.True(result.IsFailed);
            Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
        }

        [Fact]
        public async Task PinImage_pins_image_to_recipe()
        {
            await using var context = Deps.FoodStuffsContext("pin image success").Seed();
            var data = context.FoodStuffsData();

            var recipe = context.Recipes
                .Include(r => r.Images)
                .AsNoTracking()
                .First(r => r.Name == "Recipe1");

            var image = recipe.Images.First();

            var request = new PinImage.Request(image.Id);

            var result = await new PinImage.Handler(data).Handle(request);

            Assert.True(result.IsSuccess);

            var imageId = result.Value.Id;
            var pinnedImageId = context.Recipes.Where(r => r.Id == image.RecipeId).First().PinnedImageId;

            Assert.Equal(image.Id, pinnedImageId);
        }

        [Fact]
        public async Task PinImage_fails_if_image_not_found()
        {
            await using var context = Deps.FoodStuffsContext().Seed();
            var data = context.FoodStuffsData();

            var request = new PinImage.Request(-5);

            var result = await new PinImage.Handler(data).Handle(request);

            Assert.True(result.IsFailed);
            Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
        }
    }
}
