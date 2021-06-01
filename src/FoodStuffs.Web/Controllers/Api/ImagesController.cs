using FoodStuffs.Model.Events.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Web.Controllers.Api
{
    /// <summary>
    /// Manage images.
    /// </summary>
    [ApiRoute("images")]
    public class ImagesController : ControllerBase
    {
        private readonly GetImagePipeline _getPipeline;
        private readonly DeleteImagePipeline _deletePipeline;
        private readonly SaveImage.Pipeline _savePipeline;
        private readonly PinImage.Pipeline _pinPipeline;

        /// <summary>
        /// Construct a new controller.
        /// </summary>
        /// <param name="getPipeline"></param>
        /// <param name="deletePipeline"></param>
        /// <param name="savePipeline"></param>
        /// <param name="pinPipeline"></param>
        public ImagesController(GetImagePipeline getPipeline, DeleteImagePipeline deletePipeline,
            SaveImage.Pipeline savePipeline, PinImage.Pipeline pinPipeline)
        {
            _getPipeline = getPipeline;
            _deletePipeline = deletePipeline;
            _savePipeline = savePipeline;
            _pinPipeline = pinPipeline;
        }

        /// <summary>
        /// Get an image.
        /// </summary>
        /// <param name="id">The Id of the image to download.</param>
        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
        public Task<IActionResult> Get(int id)
        {
            var request = new GetImageRequest(id);

            return _getPipeline
                .Handle(request)
                .MapAsync(HttpResponder.RespondWithFile);
        }

        /// <summary>
        /// Upload an image using a multi-part form file.
        /// </summary>
        /// <param name="recipeId">The Id of the recipe the image is of</param>
        /// <param name="file">The file to upload</param>
        [HttpPost]
        [ProducesResponseType(typeof(EntityMessage<int>), 200)]
        [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
        public async Task<IActionResult> Upload(int recipeId, IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file
                .EnsureNotNull(nameof(file))
                .CopyToAsync(memoryStream)
                .ConfigureAwait(false);
            var content = memoryStream.ToArray();

            var request = new SaveImage.Request(recipeId, content);

            return await _savePipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Delete an image.
        /// </summary>
        /// <param name="id">Id of the image</param>
        [HttpDelete]
        [ProducesResponseType(typeof(EntityMessage<int>), 200)]
        [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
        public Task<IActionResult> Delete(int id)
        {
            var request = new DeleteImageRequest(id);

            return _deletePipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        /// <summary>
        /// Pin an image for a recipe. This image will be the default image for the recipe.
        /// </summary>
        /// <param name="request"></param>
        [Route("pin")]
        [HttpPost]
        [ProducesResponseType(typeof(EntityMessage<int>), 200)]
        [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
        public Task<IActionResult> Pin([FromBody] PinImage.Request request)
        {
            return _pinPipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }
    }
}
