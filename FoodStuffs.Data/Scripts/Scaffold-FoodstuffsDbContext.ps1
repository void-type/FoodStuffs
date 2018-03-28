function Scaffold-FoodstuffsDbContext {
	[CmdletBinding()]
	param(
		[Parameter(Mandatory=$true)]
		$Password
	)

	# Build the models and context
	Scaffold-DbContext "Server=SERVER;Database=FoodStuffs;User Id=FoodStuffsUser;Password=$Password;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir "$PSScriptRoot\..\Models" -Force

	# Move models to FoodStuffs.Model\Data\Models
	Move-Item -Path "$PSScriptRoot\..\Models\*" -Exclude "*Context.cs" -Destination "$PSScriptRoot\..\..\FoodStuffs.Model\Data\Models\" -Force

	Write-Host "Be sure to remove the OnConfiguring method from FoodStuffsContext as it contains sesitive information."
	Write-Host "Be sure to updates namespaces Of FoodStuffs.Model/Data/Models classes."
}