param(
    [string]$GamePath,
    [string]$TargetFile
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Update-SelectionListCommon.ps1')

if ([string]::IsNullOrWhiteSpace($GamePath)) {
    $GamePath = Get-DefaultGamePath
}

if ([string]::IsNullOrWhiteSpace($TargetFile)) {
    $TargetFile = Join-Path (Get-CinematicCameraRepoRoot) 'source\CinematicCamera\src\Actions\ActionSelectionData.cs'
}

$mountAndBladeAssemblyPath = Join-Path $GamePath 'bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.dll'
if (-not (Test-Path -Path $mountAndBladeAssemblyPath)) {
    throw "TaleWorlds.MountAndBlade.dll not found: $mountAndBladeAssemblyPath"
}

$metadataLoadContextAssemblyPath = Get-MetadataLoadContextAssemblyPath
$metadataAssembly = [System.Reflection.Assembly]::LoadFrom($metadataLoadContextAssemblyPath)
$pathAssemblyResolverType = $metadataAssembly.GetType('System.Reflection.PathAssemblyResolver', $true)
$metadataLoadContextType = $metadataAssembly.GetType('System.Reflection.MetadataLoadContext', $true)

[string[]]$gameAssemblyPaths = Get-ChildItem -Path @(
    (Join-Path $GamePath 'bin\Win64_Shipping_Client'),
    (Join-Path $GamePath 'Modules\Native\bin\Win64_Shipping_Client'),
    (Join-Path $GamePath 'Modules\SandBox\bin\Win64_Shipping_Client')
) -Filter '*.dll' -File |
    Select-Object -ExpandProperty FullName -Unique

[string[]]$runtimeAssemblyPaths = Get-ChildItem -Path ([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()) -Filter '*.dll' -File |
    Select-Object -ExpandProperty FullName

[string[]]$assemblyPaths = $gameAssemblyPaths + $runtimeAssemblyPaths
$resolverConstructor = $pathAssemblyResolverType.GetConstructors() |
    Where-Object { $_.GetParameters().Count -eq 1 } |
    Select-Object -First 1
$metadataLoadContextConstructor = $metadataLoadContextType.GetConstructors() |
    Where-Object { $_.GetParameters().Count -eq 2 } |
    Select-Object -First 1

if ($null -eq $resolverConstructor) {
    throw 'Failed to locate the PathAssemblyResolver constructor.'
}

if ($null -eq $metadataLoadContextConstructor) {
    throw 'Failed to locate the MetadataLoadContext constructor.'
}

$resolverArguments = New-Object object[] 1
$resolverArguments[0] = [string[]]$assemblyPaths
$resolver = $resolverConstructor.Invoke($resolverArguments)
$coreAssemblyName = [object].Assembly.GetName().Name
$metadataLoadContextArguments = New-Object object[] 2
$metadataLoadContextArguments[0] = $resolver
$metadataLoadContextArguments[1] = $coreAssemblyName
$metadataLoadContext = $metadataLoadContextConstructor.Invoke($metadataLoadContextArguments)

try {
    $assembly = $metadataLoadContext.LoadFromAssemblyPath($mountAndBladeAssemblyPath)
    $actionIndexCacheType = $assembly.GetType('TaleWorlds.MountAndBlade.ActionIndexCache', $true)
    $fieldNames = $actionIndexCacheType.GetFields([System.Reflection.BindingFlags] 'Public, Static') |
        Where-Object { $_.FieldType.FullName -eq 'TaleWorlds.MountAndBlade.ActionIndexCache' } |
        Sort-Object MetadataToken |
        Select-Object -ExpandProperty Name
}
finally {
    $metadataLoadContext.Dispose()
}

$methodLines = @(
    '        public static List<ActionIndexCache> GetDefaultActionList()',
    '        {',
    '            return new List<ActionIndexCache>()',
    '            {'
)

$methodLines += $fieldNames | ForEach-Object { "                ActionIndexCache.$_," }
$methodLines += @(
    '            };',
    '        }'
)

Set-CSharpMethodContent -FilePath $TargetFile -MethodSignature 'public static List<ActionIndexCache> GetDefaultActionList()' -MethodLines $methodLines
Write-Host "Updated GetDefaultActionList with $($fieldNames.Count) entries."
