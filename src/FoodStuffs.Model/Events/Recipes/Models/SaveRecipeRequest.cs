﻿namespace FoodStuffs.Model.Events.Recipes.Models;

public record SaveRecipeRequest(
    int Id,
    string Name,
    string Directions,
    string Sides,
    int? CookTimeMinutes,
    int? PrepTimeMinutes,
    bool IsForMealPlanning,
    List<SaveRecipeRequestGroceryItem> GroceryItems,
    List<string> Categories);
