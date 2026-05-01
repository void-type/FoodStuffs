using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.HomeAssistant;

/// <summary>
/// A client for interacting with the Home Assistant API.
/// </summary>
public class HomeAssistantClient(
    IHttpClientFactory httpClientFactory,
    HomeAssistantSettings settings,
    ILogger<HomeAssistantClient> logger)
{
    private HttpClient HttpClient => httpClientFactory.CreateClient("HomeAssistant");

    /// <summary>
    /// Gets the current state of the "Remind to Thaw Meat" input boolean in Home Assistant.
    /// </summary>
    public async Task<IResult<bool>> GetReminderThawMeatStateAsync()
    {
        if (!settings.Enabled)
        {
            return Result.Fail<bool>(new Failure("Home Assistant integration is disabled.", "thawMeat"));
        }

        try
        {
            var state = await GetInputBooleanStateAsync(settings.ReminderThawMeatInputBooleanEntityId);
            return Result.Ok(state);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get 'Remind to Thaw Meat' entity state from Home Assistant.");
            return Result.Fail<bool>(new Failure("Failed to get 'Remind to Thaw Meat' entity state from Home Assistant.", "thawMeat"));
        }
    }

    /// <summary>
    /// Sets the state of the "Remind to Thaw Meat" input boolean in Home Assistant, which can be used to trigger reminders for thawing meat before cooking.
    /// </summary>
    public async Task<IResult> SetReminderThawMeatAsync(bool state)
    {
        if (!settings.Enabled)
        {
            return Result.Fail(new Failure("Home Assistant integration is disabled.", "thawMeat"));
        }

        try
        {
            await SetInputBooleanEntityAsync(settings.ReminderThawMeatInputBooleanEntityId, state);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to set 'Remind to Thaw Meat' entity state in Home Assistant.");
            return Result.Fail(new Failure("Failed to set 'Remind to Thaw Meat' entity state in Home Assistant.", "thawMeat"));
        }
    }

    /// <summary>
    /// Gets the state of an input boolean entity in Home Assistant.
    /// </summary>
    /// <param name="entityId">The ID of the entity to get the state for.</param>
    /// <returns>True if the entity state is "on", false otherwise.</returns>
    private async Task<bool> GetInputBooleanStateAsync(string entityId)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentException("Entity ID cannot be null or whitespace.", nameof(entityId));
        }

        var response = await HttpClient.GetAsync($"/api/states/{entityId}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var entityState = JsonSerializer.Deserialize<HomeAssistantEntityState>(json);

        return string.Equals(entityState?.State, "on", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Sets the state of an entity in Home Assistant by calling the appropriate API endpoint with the given entity ID.
    /// </summary>
    /// <param name="entityId">The ID of the entity to set the state for.</param>
    /// <param name="state">The state to set for the entity.</param>
    private async Task SetInputBooleanEntityAsync(string entityId, bool state)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentException("Entity ID cannot be null or whitespace.", nameof(entityId));
        }

        var content = new StringContent($@"{{ ""entity_id"": ""{entityId}"" }}", Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"/api/services/input_boolean/turn_{(state ? "on" : "off")}", content);

        logger.LogInformation(
            "Set entity state request sent to Home Assistant for entity ID: {EntityId}. Desired state: {State}. Response status code: {StatusCode}. Response content:\n{ResponseContent}",
            entityId,
            state,
            response.StatusCode,
            await response.Content.ReadAsStringAsync());

        response.EnsureSuccessStatusCode();
    }

    private sealed record HomeAssistantEntityState(
        [property: JsonPropertyName("state")] string? State);
}
