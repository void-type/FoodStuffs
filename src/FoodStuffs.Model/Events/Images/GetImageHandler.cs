using FoodStuffs.Model.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Files;

namespace FoodStuffs.Model.Events.Images;

public class GetImageHandler : EventHandlerAbstract<GetImageRequest, SimpleFile>
{
    private readonly FoodStuffsContext _data;

    public GetImageHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override Task<IResult<SimpleFile>> Handle(GetImageRequest request, CancellationToken cancellationToken = default)
    {
        return _data.Images
            .Include(x => x.Blob)
            .FirstOrDefaultAsync(i => i.FileName == request.Name, cancellationToken)
            .MapAsync(Maybe.From)
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
