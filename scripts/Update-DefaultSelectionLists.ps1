param(
    [string]$GamePath
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Update-SelectionListCommon.ps1')

if ([string]::IsNullOrWhiteSpace($GamePath)) {
    $GamePath = Get-DefaultGamePath
}

& (Join-Path $PSScriptRoot 'Update-DefaultActionList.ps1') -GamePath $GamePath
& (Join-Path $PSScriptRoot 'Update-DefaultFacialAnimationList.ps1') -GamePath $GamePath
