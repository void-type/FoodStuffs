using FoodStuffs.Model.HomeAssistant;
using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Routing;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Web.Controllers.Api;

/// <summary>
/// Manage Home Assistant interactions.
/// </summary>
[Route(ApiRouteAttribute.BasePath + "/home-assistant")]
public class HomeAssistantController : ControllerBase
{
    /// <summary>
    /// Get whether Home Assistant integration is enabled. This can be used by the client to conditionally show Home Assistant-related features.
    /// </summary>
    /// <param name="settings"></param>
    [HttpGet("is-enabled")]
    [ProducesResponseType(typeof(bool), 200)]
    public IActionResult IsEnabled([FromServices] HomeAssistantSettings settings)
    {
        return HttpResponder.Respond(Result.Ok(settings.Enabled));
    }

    /// <summary>
    /// Get the current state of the "Remind to Thaw Meat" input boolean in Home Assistant.
    /// </summary>
    /// <param name="homeAssistantClient"></param>
    [HttpGet("reminder-thaw-meat-state")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> GetReminderThawMeatStateAsync([FromServices] HomeAssistantClient homeAssistantClient)
    {
        var result = await homeAssistantClient.GetReminderThawMeatStateAsync();
        return HttpResponder.Respond(result);
    }

    /// <summary>
    /// Set the state of the "Remind to Thaw Meat" input boolean in Home Assistant.
    /// </summary>
    /// <param name="homeAssistantClient"></param>
    /// <param name="state">The desired state of the "Remind to Thaw Meat" input boolean.</param>
    [HttpPost("set-reminder-thaw-meat")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(IItemSet<IFailure>), 400)]
    public async Task<IActionResult> SetReminderThawMeatAsync([FromServices] HomeAssistantClient homeAssistantClient, bool state)
    {
        var result = await homeAssistantClient.SetReminderThawMeatAsync(state);
        return HttpResponder.Respond(result);
    }
}
