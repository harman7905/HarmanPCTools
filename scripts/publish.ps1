
$ErrorActionPreference = "Stop"

$project = Join-Path $PSScriptRoot "..\HarmanPCTools\HarmanPCTools.csproj"
$output = Join-Path $PSScriptRoot "..\dist\publish"

if (Test-Path $output) { Remove-Item $output -Recurse -Force }

dotnet publish $project `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:DebugType=None `
  --output $output

Write-Host "Published to $output"
