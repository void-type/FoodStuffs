﻿namespace DataMigratorV13.OldData.Models;

public partial class MealSet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public virtual List<Recipe> Recipes { get; } = [];

    public virtual List<PantryIngredient> PantryIngredients { get; set; } = [];
}
