﻿namespace FoodStuffs.Model.Events.MealPlans.Models;

public record SaveMealPlanRequest(
    int Id,
    string Name,
    List<SaveMealPlanRequestExcludedGroceryItem> ExcludedGroceryItems,
    List<SaveMealPlanRequestRecipe> Recipes);
