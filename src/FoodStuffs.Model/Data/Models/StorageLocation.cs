﻿using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Models;

public class StorageLocation : IAuditableWithOffset
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTimeOffset ModifiedOn { get; set; }

    public virtual List<GroceryItem> GroceryItems { get; } = [];
}
