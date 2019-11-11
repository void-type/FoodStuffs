using FoodStuffs.Model.Events.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Domain;

namespace FoodStuffs.Web.Controllers.Api
{
    [ApiRoute("images")]
    public class ImagesController : ControllerBase
    {
        private readonly GetImage.Handler _getHandler;
        private readonly GetImage.Logger _getLogger;
        private readonly SaveImage.Handler _saveHandler;
        private readonly SaveImage.RequestValidator _saveValidator;
        private readonly SaveImage.Logger _saveLogger;
        private readonly DeleteImage.Handler _deleteHandler;
        private readonly DeleteImage.Logger _deleteLogger;

        public ImagesController(GetImage.Handler getHandler, GetImage.Logger getLogger,
            DeleteImage.Handler deleteHandler, DeleteImage.Logger deleteLogger,
            SaveImage.Handler updateHandler, SaveImage.RequestValidator updateValidator, SaveImage.Logger updateLogger
            )
        {
            _getHandler = getHandler;
            _getLogger = getLogger;
            _saveHandler = updateHandler;
            _saveValidator = updateValidator;
            _saveLogger = updateLogger;
            _deleteHandler = deleteHandler;
            _deleteLogger = deleteLogger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = new GetImage.Request(id);

            return await _getHandler
                .AddPostProcessor(_getLogger)
                .Handle(request)
                .MapAsync(HttpResponder.RespondWithFile);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int recipeId, IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var content = memoryStream.ToArray();

            var request = new SaveImage.Request(recipeId, content);

            return await _saveHandler
                .AddRequestValidator(_saveValidator)
                .AddPostProcessor(_saveLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteImage.Request(id);

            return await _deleteHandler
                .AddPostProcessor(_deleteLogger)
                .Handle(request)
                .MapAsync(HttpResponder.Respond);
        }
    }
}
