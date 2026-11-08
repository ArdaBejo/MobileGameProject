[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Apk,
    [Parameter(Mandatory)][string]$PreviousRelease,
    [Parameter(Mandatory)][string]$AndroidBuildTools,
    [Parameter(Mandatory)][string]$UnityOpenJdk,
    [string]$Version = '0.8.0',
    [int]$VersionCode = 5,
    [string]$PackageId = 'com.ardabecanim.mobilegame'
)

$ErrorActionPreference = 'Stop'
$apkPath = (Resolve-Path -LiteralPath $Apk).Path
$previousPath = (Resolve-Path -LiteralPath $PreviousRelease).Path
$aapt = Join-Path $AndroidBuildTools 'aapt.exe'
$java = Join-Path $UnityOpenJdk 'bin/java.exe'
$signer = Join-Path $AndroidBuildTools 'lib/apksigner.jar'

function Get-SignerFingerprint([string]$Path) {
    $lines = & $java -jar $signer verify --print-certs $Path
    if ($LASTEXITCODE) { throw "Signature verification failed: $Path" }
    $signers = @($lines | Select-String '^Signer #\d+ certificate SHA-256 digest: ([a-fA-F0-9]+)$')
    if ($signers.Count -ne 1) { throw "Expected one signer: $Path" }
    $signers[0].Matches[0].Groups[1].Value.ToLowerInvariant()
}

$metadata = & $aapt dump badging $apkPath
if ($LASTEXITCODE) { throw 'Cannot read APK manifest.' }
$packagePattern = "^package: name='$([regex]::Escape($PackageId))' versionCode='$VersionCode' versionName='$([regex]::Escape($Version))'"
if (-not ($metadata -match $packagePattern) -or ($metadata -match '^application-debuggable') -or
    -not ($metadata -match "^native-code: 'arm64-v8a'\s*$")) {
    throw 'Unexpected package, version, debug flag or architecture.'
}
if (-not ((& $aapt list $apkPath) -contains 'lib/arm64-v8a/libil2cpp.so') -or $LASTEXITCODE) {
    throw 'ARM64 IL2CPP library not found.'
}
$fingerprint = Get-SignerFingerprint $apkPath
if ($fingerprint -ne (Get-SignerFingerprint $previousPath)) { throw 'APK signer differs from previous release.' }
[pscustomobject]@{
    Package = $PackageId
    Version = $Version
    VersionCode = $VersionCode
    Architecture = 'arm64-v8a'
    Debuggable = $false
    Bytes = (Get-Item -LiteralPath $apkPath).Length
    Sha256 = (Get-FileHash -LiteralPath $apkPath -Algorithm SHA256).Hash.ToLowerInvariant()
    SignerSha256 = $fingerprint
}
