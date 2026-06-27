param(
    [ValidateSet("patch", "minor", "major")]
    [string]$BumpType = "patch"
)

$ErrorActionPreference = "Stop"

$ProjectDir = "$PSScriptRoot\VanguardMod"
$CsprojPath = "$ProjectDir\VanguardMod.csproj"
$PluginPath  = "$ProjectDir\Plugin.cs"

# --- Read & bump version ---
$csprojContent = Get-Content $CsprojPath -Raw
if ($csprojContent -match '<Version>(\d+)\.(\d+)\.(\d+)</Version>') {
    $major = [int]$Matches[1]; $minor = [int]$Matches[2]; $patch = [int]$Matches[3]
} else {
    Write-Error "Could not find <Version> in $CsprojPath"; exit 1
}

$current = "$major.$minor.$patch"
switch ($BumpType) {
    "major" { $major++; $minor = 0; $patch = 0 }
    "minor" { $minor++; $patch = 0 }
    "patch" { $patch++ }
}
$newVersion = "$major.$minor.$patch"
Write-Host "Bumping version: $current -> $newVersion"

# --- Update csproj ---
$csprojContent = $csprojContent -replace '<Version>\d+\.\d+\.\d+</Version>', "<Version>$newVersion</Version>"
Set-Content $CsprojPath $csprojContent -NoNewline

# --- Update BepInPlugin version string in Plugin.cs ---
$pluginContent = Get-Content $PluginPath -Raw
$pluginContent = $pluginContent -replace '(\[BepInPlugin\("[^"]+",\s*"[^"]+",\s*")\d+\.\d+\.\d+(")', "`${1}$newVersion`${2}"
Set-Content $PluginPath $pluginContent -NoNewline

# --- Build Release ---
Write-Host "Building Release..."
dotnet build "$CsprojPath" -c Release
if ($LASTEXITCODE -ne 0) { Write-Error "Build failed."; exit 1 }

# --- Package ---
$dllSource  = "$ProjectDir\bin\Release\net48\VGForgeStreamliner.dll"
$zipName    = "VGForgeStreamliner-v$newVersion.zip"
$zipPath    = "$PSScriptRoot\$zipName"
$stagingDir = "$env:TEMP\VGForgeStreamliner-staging"

$pluginsDir = "$stagingDir\BepInEx\plugins"
New-Item -ItemType Directory -Force -Path $pluginsDir | Out-Null
Copy-Item $dllSource -Destination "$pluginsDir\VGForgeStreamliner.dll"

if (Test-Path $zipPath) { Remove-Item $zipPath }
Compress-Archive -Path "$stagingDir\BepInEx" -DestinationPath $zipPath
Remove-Item -Recurse -Force $stagingDir

Write-Host "Created: $zipPath"

# --- GitHub Release ---
$tag = "v$newVersion"
gh release create $tag $zipPath --title "VGForgeStreamliner $tag" --generate-notes
Write-Host "Released $tag"
