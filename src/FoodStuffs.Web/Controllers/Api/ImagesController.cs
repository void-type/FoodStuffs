using FoodStuffs.Model.Events.Images;
using FoodStuffs.Model.Events.Images.Models;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage recipe images.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/images")]
public class ImagesController : ControllerBase
{
    /// <summary>
    /// Get an image.
    /// </summary>
    /// <param name="getHandler"></param>
    /// <param name="name">The name of the image to download</param>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetAsync([FromServices] GetImageHandler getHandler, string name)
    {
        var request = new GetImageRequest(name);

        return await getHandler
            .Handle(request)
            .MapAsync(HttpResponder.RespondWithInlineFile);
    }

    /// <summary>
    /// Upload an image using a multi-part form file.
    /// </summary>
    /// <param name="saveHandler"></param>
    /// <param name="recipeId">The ID of the recipe of which the image belongs to</param>
    /// <param name="file">The file to upload</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<string>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> UploadAsync([FromServices] SaveImageHandler saveHandler, int recipeId, IFormFile file)
    {
        await using var fileStream = file
            .EnsureNotNull()
            .OpenReadStream();

        var request = new SaveImageRequest(recipeId, fileStream);

        return await saveHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete an image.
    /// </summary>
    /// <param name="deleteHandler"></param>
    /// <param name="name">The name of the image to delete</param>
    [HttpDelete("{name}")]
    [ProducesResponseType(typeof(EntityMessage<string>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> DeleteAsync([FromServices] DeleteImageHandler deleteHandler, string name)
    {
        var request = new DeleteImageRequest(name);

        return await deleteHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Pin an image for a recipe. This image will be the default image for the recipe.
    /// </summary>
    /// <param name="pinHandler"></param>
    /// <param name="name">The name of the image to pin</param>
    [HttpPost("pin/{name}")]
    [ProducesResponseType(typeof(EntityMessage<string>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> PinAsync([FromServices] PinImageHandler pinHandler, string name)
    {
        var request = new PinImageRequest(name);

        return await pinHandler
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
