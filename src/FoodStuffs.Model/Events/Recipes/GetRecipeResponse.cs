using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.Events.Recipes;

public record GetRecipeResponse(
    int Id,
    string Name,
    string Directions,
    int? CookTimeMinutes,
    int? PrepTimeMinutes,
    string CreatedBy,
    DateTime CreatedOn,
    string ModifiedBy,
    DateTime ModifiedOn,
    int? PinnedImageId,
    IEnumerable<string> Categories,
    IEnumerable<int> Images,
    string Ingredients);
