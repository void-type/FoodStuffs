using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class StorageLocationNotFoundFailure : Failure
{
    public StorageLocationNotFoundFailure()
        : base(errorMessage: "Storage location not found.", uiHandle: "StorageLocationId")
    {
    }
}
