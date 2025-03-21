﻿using FoodStuffs.Model.Events;
using FoodStuffs.Model.Events.Images;
using FoodStuffs.Model.Events.Images.Models;
using FoodStuffs.Model.ImageCompression;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using VoidCore.Model.Responses.Files;
using Xunit;

namespace FoodStuffs.Test;

public class ImageEventTests
{
    [Fact]
    public async Task GetImage_gets_image_by_id_if_exists()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var image = context.Images
            .Include(a => a.ImageBlob)
            .First();

        var request = new GetImageRequest(image.FileName);

        var result = await new GetImageHandler(context).Handle(request);

        Assert.True(result.IsSuccess);

        var file = result.Value;

        Assert.Equal(image.FileName, file.Name);
        Assert.Equal(Deps.PngBase64String, Convert.ToBase64String(file.Content.AsBytes));
    }

    [Fact]
    public async Task GetImage_fails_if_image_not_foundAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var request = new GetImageRequest("not-exist-image.jpg");

        var result = await new GetImageHandler(context).Handle(request);

        Assert.True(result.IsFailed);
        Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
    }

    [Fact]
    public async Task SaveImage_creates_a_compressed_image_and_blobAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First(r => r.Name == "Hotdog");

        var myFile = new MemoryStream(Convert.FromBase64String(Deps.PngBase64String));

        var request = new SaveImageRequest(recipe.Id, myFile);
        var imageCompressor = new ImageCompressionService(new NullLogger<ImageCompressionService>());

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new SaveImageHandler(context, new NullLogger<SaveImageHandler>(), imageCompressor, indexService).Handle(request);

        Assert.True(result.IsSuccess);

        var image = context.Images.Include(a => a.ImageBlob).First(a => a.FileName == result.Value.Id);

        Assert.True(myFile.Length > image.ImageBlob.Bytes.Length);
        Assert.Equal(recipe.Id, image.RecipeId);
        Assert.Equal(Deps.DateTimeServiceLate.Moment, image.ModifiedOn);
    }

    [Fact]
    public async Task SaveImage_fails_if_file_is_not_imageAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var recipe = context.Recipes.First(r => r.Name == "Hotdog");

        var myFile = new MemoryStream(new FileContent("my file content").AsBytes);

        var request = new SaveImageRequest(recipe.Id, myFile);

        var imageCompressor = new ImageCompressionService(new NullLogger<ImageCompressionService>());

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new SaveImageHandler(context, new NullLogger<SaveImageHandler>(), imageCompressor, indexService).Handle(request);

        Assert.True(result.IsFailed);
        Assert.Contains("upload-file", result.Failures.Select(f => f.UiHandle));
    }

    [Fact]
    public async Task SaveImage_fails_if_recipe_not_foundAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var myFile = new MemoryStream(new FileContent("my file content").AsBytes);

        var request = new SaveImageRequest(-5, myFile);

        var imageCompressor = new ImageCompressionService(new NullLogger<ImageCompressionService>());

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new SaveImageHandler(context, new NullLogger<SaveImageHandler>(), imageCompressor, indexService).Handle(request);

        Assert.True(result.IsFailed);
        Assert.Contains(typeof(RecipeNotFoundFailure), result.Failures.Select(f => f.GetType()));
    }

    [Fact]
    public async Task DeleteImage_deletes_the_image_and_blobAsync()
    {
        // Due to the way we delete, we need a fresh dbcontext to remove tracked entities.
        await using var context1 = Deps.FoodStuffsContext("delete images success").Seed();

        await using var context = Deps.FoodStuffsContext("delete images success");

        var recipe = context.Recipes
            .Include(r => r.Images)
            .ThenInclude(r => r.ImageBlob)
            .AsNoTracking()
            .First(r => r.Name == "Cheeseburger");

        var image = recipe.Images.First();

        // For testing, we need to pull in all entities so EF can cascade delete.
        // In prod, SQL Server will do the cascading without needing to bring them into memory.
        var _ = context.Images.Include(x => x.ImageBlob).ToList();

        var request = new DeleteImageRequest(image.FileName);

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new DeleteImageHandler(context, indexService).Handle(request);

        Assert.True(result.IsSuccess);

        Assert.Empty(context.Images.Where(a => a.Id == image.Id).AsNoTracking().ToList());
    }

    [Fact]
    public async Task DeleteImage_fails_if_recipe_not_foundAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var request = new DeleteImageRequest("not-exist-image.jpg");

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new DeleteImageHandler(context, indexService).Handle(request);

        Assert.True(result.IsFailed);
        Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
    }

    [Fact]
    public async Task PinImage_pins_image_to_recipeAsync()
    {
        await using var context = Deps.FoodStuffsContext("pin image success").Seed();

        var recipe = context.Recipes
            .Include(r => r.Images)
            .AsNoTracking()
            .First(r => r.Name == "Cheeseburger");

        var image = recipe.Images.First();

        var request = new PinImageRequest(image.FileName);

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new PinImageHandler(context, indexService).Handle(request);

        Assert.True(result.IsSuccess);

        var pinnedImageFileName = context.Recipes
            .Where(r => r.Id == image.RecipeId)
            .First()?.PinnedImage?.FileName;

        Assert.Equal(image.FileName, pinnedImageFileName);
    }

    [Fact]
    public async Task PinImage_fails_if_image_not_foundAsync()
    {
        await using var context = Deps.FoodStuffsContext().Seed();

        var request = new PinImageRequest("not-exist-image.jpg");

        var indexService = Substitute.For<IRecipeIndexService>();

        var result = await new PinImageHandler(context, indexService).Handle(request);

        Assert.True(result.IsFailed);
        Assert.Contains(typeof(ImageNotFoundFailure), result.Failures.Select(f => f.GetType()));
    }
}
