using FoodStuffs.Model.Events.Images;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage images.
/// </summary>
[Route("api/images")]
public class ImagesController : ControllerBase
{
    /// <summary>
    /// Get an image.
    /// </summary>
    /// <param name="getPipeline"></param>
    /// <param name="name">The name of the image to download</param>
    [Route("{name}")]
    [HttpGet]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Get([FromServices] GetImagePipeline getPipeline, string name)
    {
        var request = new GetImageRequest(name);

        return getPipeline
            .Handle(request)
            .MapAsync(HttpResponder.RespondWithInlineFile);
    }

    /// <summary>
    /// Upload an image using a multi-part form file.
    /// </summary>
    /// <param name="savePipeline"></param>
    /// <param name="recipeId">The ID of the recipe of which the image belongs to</param>
    /// <param name="file">The file to upload</param>
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<string>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> Upload([FromServices] SaveImagePipeline savePipeline, int recipeId, IFormFile file)
    {
        using var fileStream = file
            .EnsureNotNull()
            .OpenReadStream();

        var request = new SaveImageRequest(recipeId, fileStream);

        return await savePipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Delete an image.
    /// </summary>
    /// <param name="deletePipeline"></param>
    /// <param name="name">The name of the image to delete</param>
    [Route("{name}")]
    [HttpDelete]
    [ProducesResponseType(typeof(EntityMessage<string>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Delete([FromServices] DeleteImagePipeline deletePipeline, string name)
    {
        var request = new DeleteImageRequest(name);

        return deletePipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }

    /// <summary>
    /// Pin an image for a recipe. This image will be the default image for the recipe.
    /// </summary>
    /// <param name="pinPipeline"></param>
    /// <param name="name">The name of the image to pin</param>
    [Route("pin/{name}")]
    [HttpPost]
    [ProducesResponseType(typeof(EntityMessage<string>), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public Task<IActionResult> Pin([FromServices] PinImagePipeline pinPipeline, string name)
    {
        var request = new PinImageRequest(name);

        return pinPipeline
            .Handle(request)
            .MapAsync(HttpResponder.Respond);
    }
}
