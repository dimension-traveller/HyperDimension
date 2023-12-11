param (
  [String] $MigrationName = $(throw "MigrationName parameter is required.")
)

$ProjectRoot = (Split-Path -Path $PSScriptRoot -Parent)

Write-Output $ProjectRoot

$StartUpProjectName = "HyperDimension.Presentation.Api"
$DbContextProjectName = "HyperDimension.Infrastructure.Database"

$StartUpProject = [System.IO.Path]::Join($ProjectRoot, "src/$StartUpProjectName/$StartUpProjectName.csproj")
$DbContextProject = [System.IO.Path]::Join($ProjectRoot, "src/$DbContextProjectName/$DbContextProjectName.csproj")
$DbContext = "$DbContextProjectName.HyperDimensionDbContext"

$Message = @{
  StartUpProject = $StartUpProject
  DbContextProject = $DbContextProject
  DbContext = $DbContext
}

Write-Output -InputObject $Message

dotnet ef migrations add $MigrationName -p $DbContextProject -s $StartUpProject -c $DbContext
