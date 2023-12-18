param (
  [String] $MigrationName = $(throw "MigrationName parameter is required.")
)

$ProjectRoot = (Split-Path -Path $PSScriptRoot -Parent)

Write-Output $ProjectRoot

$StartUpProjectName = "HyperDimension.Presentation.Api"
$DbContextProjectName = "HyperDimension.Infrastructure.Database"

$StartUpProject = [System.IO.Path]::Join($ProjectRoot, "src/$StartUpProjectName/$StartUpProjectName.csproj")
$DbContext = "$DbContextProjectName.HyperDimensionDbContext"

$Message = @{
  StartUpProject = $StartUpProject
  DbContext      = $DbContext
}

$Databases = @{
  SQLite     = "Data Source=:memory:;"
  SQLServer  = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"
  PostgreSQL = "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;"
  MySQL      = "Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd=myPassword;"
}

Write-Output -InputObject $Message

dotnet build -c Release

[System.Environment]::SetEnvironmentVariable("HD_DEBUG_FORCE_DEFAULT_MYSQL", "true")

foreach ($Database in $Databases.GetEnumerator()) {
  $DatabaseName = $Database.Key
  $ConnectionString = $Database.Value

  $MigrationProject = [System.IO.Path]::Join($ProjectRoot, "src/HyperDimension.Migrations.$DatabaseName/HyperDimension.Migrations.$DatabaseName.csproj")

  Write-Output -InputObject "Database: $DatabaseName"
  Write-Output -InputObject "ConnectionString: $ConnectionString"
  Write-Output -InputObject "MigrationProject: $MigrationProject"

  Write-Output -InputObject ""

  dotnet ef migrations add $MigrationName -p $MigrationProject -s $StartUpProject -c $DbContext --no-build --configuration Release -- --Database:Type=$DatabaseName --Database:ConnectionString=$ConnectionString
}
