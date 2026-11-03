#Requires -Version 7.4
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Adb,
    [Parameter(Mandatory)][ValidatePattern('^[\w.:-]+$')][string]$Serial,
    [Parameter(Mandatory)][string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'
$adbExe = (Get-Item -LiteralPath $Adb).FullName
$evidencePath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutputDirectory)
if (Test-Path -LiteralPath $evidencePath) {
    throw 'Choose a new output directory to avoid overwriting previous evidence.'
}
$deviceState = & $adbExe -s $Serial get-state
if ($LASTEXITCODE -ne 0 -or $deviceState -ne 'device') {
    throw "Android target $Serial is not ready."
}
New-Item -ItemType Directory -Path $evidencePath | Out-Null
$timeline = Join-Path $evidencePath 'timeline.txt'

function Write-Marker([string]$Message) {
    "$(Get-Date -Format o) $Message" | Add-Content -LiteralPath $timeline
}

function Save-Screenshot([string]$Name) {
    $imagePath = Join-Path $evidencePath $Name
    & $adbExe -s $Serial exec-out screencap -p > $imagePath
    if ($LASTEXITCODE -ne 0 -or (Get-Item -LiteralPath $imagePath).Length -lt 8) {
        throw "ADB screenshot failed: $Name"
    }
    Write-Marker "$Name saved"
}

$log = Start-Process -FilePath $adbExe -ArgumentList @('-s', $Serial, 'logcat', '-T', '1', '-v', 'time', '-s', 'Unity') `
    -RedirectStandardOutput (Join-Path $evidencePath 'logcat-soak.txt') `
    -RedirectStandardError (Join-Path $evidencePath 'logcat-errors.txt') -WindowStyle Hidden -PassThru
try {
    Write-Marker "Soak started on $Serial; player controls gameplay."
    $timer = [Diagnostics.Stopwatch]::StartNew()
    Save-Screenshot 'minute-00.png'
    foreach ($minute in 5, 10) {
        while ($timer.Elapsed.TotalSeconds -lt 60 * $minute) {
            if ($log.HasExited) { throw 'ADB logcat stopped before the end of the soak.' }
            Start-Sleep -Milliseconds 250
        }
        if ($minute -eq 10) {
            Write-Marker 'End battery sample'
            & $adbExe -s $Serial shell dumpsys battery | Set-Content -LiteralPath (Join-Path $evidencePath 'battery-after.txt')
            if ($LASTEXITCODE -ne 0) { throw 'End battery capture failed.' }
        }
        Save-Screenshot ('minute-{0:D2}.png' -f $minute)
    }
    Write-Marker 'Soak collection complete; verify overlay and foreground gameplay in screenshots.'
}
finally {
    if (-not $log.HasExited) { Stop-Process -InputObject $log }
    $log.Dispose()
}
