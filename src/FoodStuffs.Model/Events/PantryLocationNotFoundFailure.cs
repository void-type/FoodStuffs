using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class PantryLocationNotFoundFailure : Failure
{
    public PantryLocationNotFoundFailure()
        : base(errorMessage: "Pantry location not found.", uiHandle: "PantryLocationId")
    {
    }
}
