
$ErrorActionPreference = "Stop"

& (Join-Path $PSScriptRoot "publish.ps1")

$project = Join-Path $PSScriptRoot "..\HarmanPCTools\HarmanPCTools.csproj"
$iss = Join-Path $PSScriptRoot "..\installer\HarmanPCTools.iss"

[xml]$xml = Get-Content $project
$version = $xml.Project.PropertyGroup.Version

$iscc = @(
  "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
  "$env:ProgramFiles\Inno Setup 6\ISCC.exe"
) | Where-Object { $_ -and (Test-Path $_) } | Select-Object -First 1

if (-not $iscc) { throw "Inno Setup 6 not found." }

& $iscc "/DMyAppVersion=$version" $iss
Write-Host "Installer built."
