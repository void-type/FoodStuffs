using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class PantryLocationNotFoundFailure : Failure
{
    public PantryLocationNotFoundFailure()
        : base(errorMessage: "Pantry Location not found.", uiHandle: "PantryLocationId")
    {
    }
}
