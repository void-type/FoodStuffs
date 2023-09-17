using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Files;

namespace FoodStuffs.Model.Events.Images;

public class GetImageHandler : EventHandlerAbstract<GetImageRequest, SimpleFile>
{
    private readonly IFoodStuffsData _data;

    public GetImageHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override Task<IResult<SimpleFile>> Handle(GetImageRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new ImagesByNameWithBlobsSpecification(request.Name);

        return _data.Images.Get(byId, cancellationToken)
            .ToResultAsync(new ImageNotFoundFailure())
            .ThenAsync(ValidateBlobIsNotNull)
            .SelectAsync(r => new SimpleFile(r.Blob!.Bytes, r.FileName));
    }

    private static IResult<Image> ValidateBlobIsNotNull(Image r)
    {
        return r.Blob is not null ?
            Result.Ok(r) :
            Result.Fail<Image>(new ImageNotFoundFailure());
    }
}
