function Scaffold-FoodstuffsDbContext {
	[CmdletBinding()]
	param(
		[Parameter(Mandatory=$true)]
		$Password
	)

	# Run this script from the root of the concrete services project.

	$contextDirectory = "Data\EntityFramework*"

	# Build the models and context
	dotnet ef dbcontext scaffold "Server=SERVER1;Database=FoodStuffs;User Id=FoodStuffsUser;Password=$Password;" Microsoft.EntityFrameworkCore.SqlServer -o "$contextDirectory" -f
	
	# Move models to FoodStuffs.Model\Data\Models
	Move-Item -Path "$contextDirectory" -Exclude "*Context.cs" -Destination "..\FoodStuffs.Model\Data\Models\" -Force

	Write-Host "Be sure to remove the OnConfiguring method from FoodStuffsContext as it contains sesitive information."
	Write-Host "Be sure to updates namespaces Of FoodStuffs.Model/Data/Models classes."
}