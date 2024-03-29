﻿namespace DataMigratorV13.OldData.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<Recipe> Recipes { get; } = [];
}
