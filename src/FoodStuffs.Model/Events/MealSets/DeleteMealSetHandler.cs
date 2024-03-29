﻿using FoodStuffs.Model.Data;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class DeleteMealSetHandler : CustomEventHandlerAbstract<DeleteMealSetRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public DeleteMealSetHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override Task<IResult<EntityMessage<int>>> Handle(DeleteMealSetRequest request, CancellationToken cancellationToken = default)
    {
        return _data.MealSets
            .TagWith(GetTag())
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new MealSetNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                _data.MealSets.Remove(r);
                await _data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Meal set deleted.", r.Id));
    }
}
