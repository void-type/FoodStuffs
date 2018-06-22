function Scaffold-FoodstuffsDbContext {
	[CmdletBinding()]
	param(
	)

	# Run this script from the root of the concrete services project.

	$contextDirectory = "Data/EntityFramework"

	$connectionString = Get-Content -Path "../FoodStuffs.Web/appsettings.Development.json" | ConvertFrom-Json | Select-Object -ExpandProperty ConnectionString

	# Build the models and context
	dotnet ef dbcontext scaffold "$connectionString" Microsoft.EntityFrameworkCore.SqlServer -o "$contextDirectory" -f
	
	# Move models to FoodStuffs.Model\Data\Models
	Move-Item -Path "$contextDirectory/*" -Exclude "*Context.cs" -Destination "../FoodStuffs.Model/Data/Models/" -Force

	Write-Host "Be sure to remove the OnConfiguring method from FoodStuffsContext as it contains sesitive information."
	Write-Host "Be sure to updates namespaces Of FoodStuffs.Model/Data/Models classes."
}
