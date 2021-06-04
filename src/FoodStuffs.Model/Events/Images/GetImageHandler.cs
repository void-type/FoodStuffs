using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Files;

namespace FoodStuffs.Model.Events.Images
{
    public class GetImageHandler : EventHandlerAbstract<GetImageRequest, SimpleFile>
    {
        private readonly IFoodStuffsData _data;

        public GetImageHandler(IFoodStuffsData data)
        {
            _data = data;
        }

        public override Task<IResult<SimpleFile>> Handle(GetImageRequest request, CancellationToken cancellationToken = default)
        {
            var byId = new ImagesByIdWithBlobsSpecification(request.Id);

            return _data.Images.Get(byId, cancellationToken)
                .ToResultAsync(new ImageNotFoundFailure())
                .SelectAsync(r => new SimpleFile(r.Blob.Bytes, $"{r.Id}"));
        }
    }
}
