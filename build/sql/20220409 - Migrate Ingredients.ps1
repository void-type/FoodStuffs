$recipesTable = Import-Csv "$PSScriptRoot/data.csv"

$recipesTable |
  ForEach-Object {
    $recipe = $_

    $ingredients = $recipe.Ingredients -split ('\r?\n') | ? { $_ -ne "" }

    for ($i = 0; $i -lt $ingredients.Count; $i++) {
      $ing = $ingredients[$i].Replace('''', '''''')
      "INSERT INTO [dbo].[Ingredient] ([Name],[Quantity],[Order],[IsCategory],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[RecipeId])
VALUES
('$ing'
,1
,$($i + 1)
,0
,'$($recipe.CreatedBy)'
,CAST(N'$($recipe.CreatedOn)' AS DateTime2)
,'$($recipe.ModifiedBy)'
,CAST(N'$($recipe.ModifiedOn)' AS DateTime2)
,$($recipe.Id))
"
    }
  } |
  Out-File "./20220409 - Migrate Ingredients.output.sql"
