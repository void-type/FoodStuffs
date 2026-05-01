namespace FoodStuffs.Model.HomeAssistant;

public class HomeAssistantSettings
{
    public bool Enabled { get; set; } = false;
    public string ApiEndpoint { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string ReminderThawMeatInputBooleanEntityId { get; set; } = string.Empty;
}
