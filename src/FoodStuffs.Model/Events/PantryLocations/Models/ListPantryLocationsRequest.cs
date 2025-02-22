using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.PantryLocations.Models;

public record ListPantryLocationsRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
