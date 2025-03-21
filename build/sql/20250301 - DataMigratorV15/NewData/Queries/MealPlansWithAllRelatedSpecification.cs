﻿using DataMigratorV15.NewData.Models;
using VoidCore.Model.Data;

namespace DataMigratorV15.NewData.Queries;

public class MealPlansWithAllRelatedSpecification : QuerySpecificationAbstract<MealPlan>
{
    public MealPlansWithAllRelatedSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        IncludeAll();
    }

    private void IncludeAll()
    {
        AddInclude($"{nameof(MealPlan.PantryShoppingItemRelations)}.{nameof(MealPlanPantryShoppingItemRelation.ShoppingItem)}");

        var recipe = $"{nameof(MealPlan.RecipeRelations)}.{nameof(MealPlanRecipeRelation.Recipe)}";
        AddInclude($"{recipe}.{nameof(Recipe.Categories)}");
        AddInclude($"{recipe}.{nameof(Recipe.Images)}");
        AddInclude($"{recipe}.{nameof(Recipe.PinnedImage)}");
        AddInclude($"{recipe}.{nameof(Recipe.ShoppingItemRelations)}.{nameof(RecipeShoppingItemRelation.ShoppingItem)}");
    }
}
