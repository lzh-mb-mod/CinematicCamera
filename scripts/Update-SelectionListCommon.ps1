Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-CinematicCameraRepoRoot {
    return (Split-Path -Path $PSScriptRoot -Parent)
}

function Get-DefaultGamePath {
    param(
        [string]$RepoRoot = (Get-CinematicCameraRepoRoot)
    )

    $propsPath = Join-Path $RepoRoot 'source\library\mission-library\source\BasicSharedLibrary\GamePath.props'
    if (-not (Test-Path -Path $propsPath)) {
        throw "Game path props file not found: $propsPath"
    }

    [xml]$props = Get-Content -Path $propsPath
    $gamePath = $props.Project.PropertyGroup.GamePath
    if ([string]::IsNullOrWhiteSpace($gamePath)) {
        throw "GamePath is missing in $propsPath"
    }

    return $gamePath.TrimEnd('\')
}

function Get-MetadataLoadContextAssemblyPath {
    $sdkRoot = 'C:\Program Files\dotnet\sdk'
    if (-not (Test-Path -Path $sdkRoot)) {
        throw "dotnet SDK directory not found: $sdkRoot"
    }

    $preferredMajorVersion = [Environment]::Version.Major
    $sdkCandidates = Get-ChildItem -Path $sdkRoot -Directory |
        Where-Object { $_.Name -match '^\d+\.\d+\.\d+$' } |
        ForEach-Object {
            [pscustomobject]@{
                Version = [version]$_.Name
                Path = Join-Path $_.FullName 'System.Reflection.MetadataLoadContext.dll'
            }
        } |
        Where-Object { Test-Path -Path $_.Path }

    $assemblyPath = $sdkCandidates |
        Where-Object { $_.Version.Major -eq $preferredMajorVersion } |
        Sort-Object Version -Descending |
        Select-Object -First 1 -ExpandProperty Path

    if ([string]::IsNullOrWhiteSpace($assemblyPath)) {
        $assemblyPath = $sdkCandidates |
            Sort-Object Version -Descending |
            Select-Object -First 1 -ExpandProperty Path
    }

    if ([string]::IsNullOrWhiteSpace($assemblyPath)) {
        throw 'System.Reflection.MetadataLoadContext.dll was not found under the dotnet SDK directory.'
    }

    return $assemblyPath
}

function Set-CSharpMethodContent {
    param(
        [Parameter(Mandatory = $true)]
        [string]$FilePath,

        [Parameter(Mandatory = $true)]
        [string]$MethodSignature,

        [Parameter(Mandatory = $true)]
        [string[]]$MethodLines
    )

    if (-not (Test-Path -Path $FilePath)) {
        throw "Target file not found: $FilePath"
    }

    [string[]]$lines = Get-Content -Path $FilePath
    $startIndex = -1
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i].Trim() -eq $MethodSignature) {
            $startIndex = $i
            break
        }
    }

    if ($startIndex -lt 0) {
        throw "Method signature not found in $FilePath : $MethodSignature"
    }

    $braceBalance = 0
    $foundOpeningBrace = $false
    $endIndex = -1

    for ($i = $startIndex; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        $braceBalance += ([regex]::Matches($line, '\{')).Count
        if ($line.Contains('{')) {
            $foundOpeningBrace = $true
        }

        $braceBalance -= ([regex]::Matches($line, '\}')).Count

        if ($foundOpeningBrace -and $braceBalance -eq 0) {
            $endIndex = $i
            break
        }
    }

    if ($endIndex -lt 0) {
        throw "Failed to locate the end of method $MethodSignature in $FilePath"
    }

    [string[]]$before = @()
    if ($startIndex -gt 0) {
        $before = $lines[0..($startIndex - 1)]
    }

    [string[]]$after = @()
    if ($endIndex + 1 -lt $lines.Count) {
        $after = $lines[($endIndex + 1)..($lines.Count - 1)]
    }

    [string[]]$updatedLines = @($before + $MethodLines + $after)
    $updatedContent = ($updatedLines -join "`r`n") + "`r`n"
    $utf8Bom = [System.Text.UTF8Encoding]::new($true)
    [System.IO.File]::WriteAllText($FilePath, $updatedContent, $utf8Bom)
}
