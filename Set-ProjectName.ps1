# Prompt the user for the old prefix with a default value
$defaultOldPrefix = "CleanBlazor"
$userInputOldPrefix = Read-Host "Enter the old project name (default: '$defaultOldPrefix')"
$oldPrefix = if ($userInputOldPrefix) { $userInputOldPrefix } else { $defaultOldPrefix }

# Prompt for the new prefix
$newPrefix = Read-Host "Enter the new project name to replace '$oldPrefix'"

# Use $PSScriptRoot to get the directory where the script is located
$directory = $PSScriptRoot

Get-ChildItem -Path $directory -Recurse -Exclude obj, bin -File |
ForEach-Object {
    $fileContent = Get-Content $_.FullName
    if ($fileContent -match $oldPrefix) {
        $fileContent.Replace($oldPrefix, $newPrefix) | Set-Content $_.FullName
    }
}

Write-Host "Project name set to '$newPrefix'."
