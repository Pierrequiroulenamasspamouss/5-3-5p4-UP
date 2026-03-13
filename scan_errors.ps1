$ErrorActionPreference = "SilentlyContinue"
$patterns = @(
    "base.IsClosed",
    "SetHandleAsInvalid",
    "File.ReadAllBytes",
    "File.ReadAllText",
    "File.WriteAllBytes",
    "File.Move",
    "File.Delete",
    "File.Exists",
    "File.OpenRead",
    "File.Create",
    "Directory.CreateDirectory",
    "Directory.Exists",
    "DirectoryInfo.GetFiles",
    "FileInfo.Length",
    "FileInfo.Delete",
    "FileInfo.OpenRead",
    "FileInfo.Directory",
    "Process.StartTime",
    "AesManaged"
)

$results = @()
foreach ($pattern in $patterns) {
    # Match global::System.IO.Pattern OR just Pattern
    # This might have more false positives but is safer
    $regex = "($pattern)"
    if ($pattern -in @("base.IsClosed", "SetHandleAsInvalid", "Process.StartTime", "AesManaged")) {
        $regex = $pattern
    }
    else {
        $regex = "(global::System\.IO\.)?$pattern"
    }

    $matches = Get-ChildItem -Path "Minionsparadise/Assets/Scripts" -Filter "*.cs" -Recurse | Select-String -Pattern $regex
    
    foreach ($match in $matches) {
        # Extract the line and check if it's already wrapped
        $line = $match.Line.Trim()
        
        # Simple check for same-line wrapping
        if ($line -match "#if" -or $line -match "#else" -or $line -match "#endif") {
            continue
        }
        
        # Check context (previous line for #if)
        $fileLines = Get-Content $match.Path
        $lineIndex = $match.LineNumber - 1
        $isWrapped = $false
        
        # Look back up to 100 lines for an #if
        for ($i = [Math]::Max(0, $lineIndex - 100); $i -le $lineIndex; $i++) {
            if ($fileLines[$i] -match "#if\s+!UNITY_WEBPLAYER") {
                $isWrapped = $true
                break
            }
        }
        
        if (-not $isWrapped) {
            $results += "$($match.Path):$($match.LineNumber): $line"
        }
    }
}

Write-Host "Found $($results.Count) potential issues:"
$results | Out-File -FilePath "audit_results.txt" -Encoding utf8
Write-Host "Results saved to audit_results.txt"
