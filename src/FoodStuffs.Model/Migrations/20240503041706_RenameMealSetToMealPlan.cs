using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations
{
    /// <inheritdoc />
    public partial class RenameMealSetToMealPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealSetRecipe_MealSet_MealSetsId",
                table: "MealSetRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_MealSetRecipe_Recipe_RecipesId",
                table: "MealSetRecipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealSetRecipe",
                table: "MealSetRecipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealSet",
                table: "MealSet");

            migrationBuilder.RenameTable(
                name: "MealSetRecipe",
                newName: "MealPlanRecipe");

            migrationBuilder.RenameTable(
                name: "MealSet",
                newName: "MealPlan");

            migrationBuilder.RenameIndex(
                name: "IX_MealSetRecipe_RecipesId",
                table: "MealPlanRecipe",
                newName: "IX_MealPlanRecipe_RecipesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealPlanRecipe",
                table: "MealPlanRecipe",
                columns: new[] { "MealSetsId", "RecipesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealPlan",
                table: "MealPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanRecipe_MealPlan_MealSetsId",
                table: "MealPlanRecipe",
                column: "MealSetsId",
                principalTable: "MealPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanRecipe_Recipe_RecipesId",
                table: "MealPlanRecipe",
                column: "RecipesId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanRecipe_MealPlan_MealSetsId",
                table: "MealPlanRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanRecipe_Recipe_RecipesId",
                table: "MealPlanRecipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealPlanRecipe",
                table: "MealPlanRecipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealPlan",
                table: "MealPlan");

            migrationBuilder.RenameTable(
                name: "MealPlanRecipe",
                newName: "MealSetRecipe");

            migrationBuilder.RenameTable(
                name: "MealPlan",
                newName: "MealSet");

            migrationBuilder.RenameIndex(
                name: "IX_MealPlanRecipe_RecipesId",
                table: "MealSetRecipe",
                newName: "IX_MealSetRecipe_RecipesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealSetRecipe",
                table: "MealSetRecipe",
                columns: new[] { "MealSetsId", "RecipesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealSet",
                table: "MealSet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealSetRecipe_MealSet_MealSetsId",
                table: "MealSetRecipe",
                column: "MealSetsId",
                principalTable: "MealSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealSetRecipe_Recipe_RecipesId",
                table: "MealSetRecipe",
                column: "RecipesId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
