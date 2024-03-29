﻿using Microsoft.Extensions.Logging;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Images;

public class PinImageResponseLogger : EntityMessageEventLogger<PinImageRequest, string>
{
    public PinImageResponseLogger(ILogger<PinImageResponseLogger> logger) : base(logger) { }
}
