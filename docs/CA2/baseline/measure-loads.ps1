param(
    [string]$AdbPath = 'D:/UNITY/6000.6.0f1/Editor/Data/PlaybackEngines/AndroidPlayer/SDK/platform-tools/adb.exe',
    [string]$Serial = 'emulator-5554',
    [string]$Package = 'com.ardabecanim.mobilegame',
    [string]$ApkPath = 'Builds/MyGame-0.4.0.apk',
    [string]$EvidenceName = 'w05-load-times'
)

$ErrorActionPreference = 'Stop'
function Invoke-Device([string[]]$DeviceArguments) {
    $response = & $AdbPath -s $Serial @DeviceArguments 2>&1
    if ($LASTEXITCODE -ne 0) { throw ($response -join "`n") }
    return ($response -join "`n")
}

$apk = Get-Item -LiteralPath $ApkPath
$localHash = (Get-FileHash -LiteralPath $apk.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
$installedPath = ((Invoke-Device @('shell', 'pm', 'path', $Package)) -split "`n" | Where-Object { $_ -match 'base\.apk' } | Select-Object -First 1) -replace '^package:', ''
$installedPath = $installedPath.Trim()
$remoteHash = ((Invoke-Device @('shell', 'sha256sum', $installedPath)) -split '\s+')[0].ToLowerInvariant()
if ($localHash -ne $remoteHash) { throw 'Installed APK differs from the specified local APK. Rebuild/install before measuring.' }
$activity = (Invoke-Device @('shell', 'cmd', 'package', 'resolve-activity', '--brief', $Package)) -split "`n" | ForEach-Object { $_.Trim() } | Where-Object { $_ -match '^\S+/\S+$' } | Select-Object -Last 1
if (!$activity) { throw 'Could not resolve launch activity.' }
$version = Invoke-Device @('shell', 'dumpsys', 'package', $Package)
$android = (Invoke-Device @('shell', 'getprop', 'ro.build.version.release')).Trim()
$model = (Invoke-Device @('shell', 'getprop', 'ro.product.model')).Trim()
$raw = [Collections.Generic.List[string]]::new()
$trials = @()

for ($trial = 1; $trial -le 3; $trial++) {
    $null = Invoke-Device @('shell', 'am', 'force-stop', $Package)
    $launch = Invoke-Device @('shell', 'am', 'start', '-W', '-n', $activity)
    $total = [regex]::Match($launch, '(?m)^TotalTime:\s*(\d+)')
    if (!$total.Success) { throw ('TotalTime missing: ' + $launch) }
    $processId = (Invoke-Device @('shell', 'pidof', $Package)).Trim()
    if ($processId -notmatch '^\d+$') { throw 'Expected exactly one application process.' }
    $timeout = [Diagnostics.Stopwatch]::StartNew()
    do {
        $log = Invoke-Device @('logcat', '-d', '--pid', $processId, '-s', 'Unity:I', '*:S')
        $interactive = [regex]::Match($log, '\[perf\] interactive ([0-9.]+)s')
        $menu = [regex]::Match($log, '\[perf\] scene Menu loaded in (\d+) ms')
        if ($interactive.Success -and $menu.Success) { break }
        if ($timeout.Elapsed.TotalSeconds -gt 25) { throw 'Timed out waiting for MenuReady and SceneLoader markers. Verify the rebuilt APK.' }
        Start-Sleep -Milliseconds 250
    } while ($true)
    $perf = ($log -split "`n" | Where-Object { $_ -match '\[perf\]|Build type|ApplicationInfo' }) -join "`n"
    $raw.Add("Trial $trial, PID $processId`n$launch`n$perf`n")
    $trialData = [ordered]@{
        trial = $trial
        pid = [int]$processId
        totalTimeMs = [int]$total.Groups[1].Value
        interactiveSeconds = [double]::Parse($interactive.Groups[1].Value, [Globalization.CultureInfo]::InvariantCulture)
        menuSceneLoadMs = [int]$menu.Groups[1].Value
    }
    $trials += [pscustomobject]$trialData
    Write-Output ($trialData | ConvertTo-Json -Compress)
}

$result = [ordered]@{
    deviceSerial = $Serial
    deviceModel = $model
    androidVersion = $android
    package = $Package
    activity = $activity
    versionName = [regex]::Match($version, 'versionName=(\S+)').Groups[1].Value
    versionCode = [regex]::Match($version, 'versionCode=(\d+)').Groups[1].Value
    apkPath = $apk.FullName
    apkBytes = $apk.Length
    apkSha256 = $localHash
    installedApkMatches = $true
    trials = $trials
    medianColdStartMs = @($trials.totalTimeMs | Sort-Object)[1]
    medianInteractiveSeconds = @($trials.interactiveSeconds | Sort-Object)[1]
    medianMenuSceneLoadMs = @($trials.menuSceneLoadMs | Sort-Object)[1]
}
$evidencePath = Join-Path $PSScriptRoot ($EvidenceName + '.json')
[IO.File]::WriteAllText($evidencePath, ($result | ConvertTo-Json -Depth 5) + "`n", [Text.UTF8Encoding]::new($false))
[IO.File]::WriteAllText((Join-Path $PSScriptRoot ($EvidenceName + '.txt')), ($raw -join "`n"), [Text.UTF8Encoding]::new($false))
Write-Output ('Evidence: ' + $evidencePath)
