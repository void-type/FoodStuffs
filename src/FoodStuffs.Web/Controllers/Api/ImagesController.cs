using FoodStuffs.Model.Events.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("images")]
    public class ImagesController : ControllerBase
    {
        private readonly GetImage.Pipeline _getPipeline;
        private readonly DeleteImage.Pipeline _deletePipeline;
        private readonly SaveImage.Pipeline _savePipeline;
        private readonly PinImage.Pipeline _pinPipeline;

        public ImagesController(GetImage.Pipeline getPipeline, DeleteImage.Pipeline deletePipeline,
            SaveImage.Pipeline savePipeline, PinImage.Pipeline pinPipeline)
        {
            _getPipeline = getPipeline;
            _deletePipeline = deletePipeline;
            _savePipeline = savePipeline;
            _pinPipeline = pinPipeline;
        }

        [Route("{id}")]
        [HttpGet]
        public Task<IActionResult> Get(int id)
        {
            var request = new GetImage.Request(id);

            return _getPipeline
                .Handle(request)
                .MapAsync(HttpResponder.RespondWithFile);
        }

        [HttpPost]
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

        [HttpDelete]
        public Task<IActionResult> Delete(int id)
        {
            var request = new DeleteImage.Request(id);

            return _deletePipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        [Route("pin")]
        [HttpPost]
        public Task<IActionResult> Pin([FromBody] PinImage.Request request)
        {
            return _pinPipeline
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }
    }
}
