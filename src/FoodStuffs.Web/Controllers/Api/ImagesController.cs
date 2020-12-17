using FoodStuffs.Model.Events.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Domain;
using VoidCore.Domain.Guards;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("images")]
    public class ImagesController : ControllerBase
    {
        private readonly GetImage.Handler _getHandler;
        private readonly GetImage.Logger _getLogger;
        private readonly SaveImage.Handler _saveHandler;
        private readonly SaveImage.Logger _saveLogger;
        private readonly DeleteImage.Handler _deleteHandler;
        private readonly DeleteImage.Logger _deleteLogger;
        private readonly PinImage.Handler _pinHandler;
        private readonly PinImage.Logger _pinLogger;

        public ImagesController(GetImage.Handler getHandler, GetImage.Logger getLogger,
            DeleteImage.Handler deleteHandler, DeleteImage.Logger deleteLogger,
            SaveImage.Handler saveHandler, SaveImage.Logger saveLogger,
            PinImage.Handler pinHandler, PinImage.Logger pinLogger
            )
        {
            _getHandler = getHandler;
            _getLogger = getLogger;
            _saveHandler = saveHandler;
            _saveLogger = saveLogger;
            _deleteHandler = deleteHandler;
            _deleteLogger = deleteLogger;
            _pinHandler = pinHandler;
            _pinLogger = pinLogger;
        }

        [Route("{id}")]
        [HttpGet]
        public Task<IActionResult> Get(int id)
        {
            var request = new GetImage.Request(id);

            return _getHandler
                .AddPostProcessor(_getLogger)
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

            return await _saveHandler
                .AddPostProcessor(_saveLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond)
                .ConfigureAwait(false);
        }

        [HttpDelete]
        public Task<IActionResult> Delete(int id)
        {
            var request = new DeleteImage.Request(id);

            return _deleteHandler
                .AddPostProcessor(_deleteLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        [Route("pin")]
        [HttpPost]
        public Task<IActionResult> Pin([FromBody] PinImage.Request request)
        {
            return _pinHandler
                .AddPostProcessor(_pinLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }
    }
}
