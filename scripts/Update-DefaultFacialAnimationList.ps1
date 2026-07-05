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
    $TargetFile = Join-Path (Get-CinematicCameraRepoRoot) 'source\CinematicCamera\src\FacialAnimation\FacialAnimationSelectionData.cs'
}

$voicesXmlPath = Join-Path $GamePath 'Modules\Native\ModuleData\voices.xml'
if (-not (Test-Path -Path $voicesXmlPath)) {
    throw "voices.xml not found: $voicesXmlPath"
}

[xml]$voicesXml = Get-Content -Path $voicesXmlPath
$recordIds = $voicesXml.base.face_animation_records.face_animation_record |
    ForEach-Object { $_.id } |
    Where-Object { -not [string]::IsNullOrWhiteSpace($_) }

$methodLines = @(
    '        public static List<string> GetDefaultFacialAnimationList()',
    '        {',
    '            return new List<string>()',
    '            {'
)

$methodLines += $recordIds | ForEach-Object { "                `"$_`"," }
$methodLines += @(
    '            };',
    '        }'
)

Set-CSharpMethodContent -FilePath $TargetFile -MethodSignature 'public static List<string> GetDefaultFacialAnimationList()' -MethodLines $methodLines
Write-Host "Updated GetDefaultFacialAnimationList with $($recordIds.Count) entries."
