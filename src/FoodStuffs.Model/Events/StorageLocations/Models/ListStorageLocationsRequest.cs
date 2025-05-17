using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.StorageLocations.Models;

public record ListStorageLocationsRequest(
    string? NameSearch,
    bool? IsUnused,
    bool IsPagingEnabled,
    int Page,
    int Take) : IPaginatedRequest;
