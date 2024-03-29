﻿using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class DeleteImageResponseLogger : EntityMessageEventLogger<DeleteImageRequest, string>
{
    public DeleteImageResponseLogger(ILogger<DeleteImageResponseLogger> logger) : base(logger) { }
}
