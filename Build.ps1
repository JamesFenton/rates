param(
	$buildVersion = "0.0.1",
	$buildCounter = 0,
	$artifactDirectory = "$PSScriptRoot\artifacts",
	$applicationInsightsKey = $null
)

function Test-ExitCode($exitCode) {
	if ($exitCode -ne 0) {
		exit $exitCode
	}
}

function Replace-Text($file, $replacementToken, $value) {
	$file = (Get-Content $file) -join "`n"
	$file.Replace($replacementToken, $value) | Out-File $file
}

$version = "$buildVersion.$buildCounter"
$functionsPublishDirectory = "$PSScriptRoot\publish\Rates.Functions"

if ($applicationInsightsKey -ne $null) {
	Replace-Text "$PSScriptRoot\src\Rates.Functions\wwwroot\index.html" "<insert instrumentation key>" $applicationInsightsKey
}

# publish web
dotnet publish "$PSScriptRoot\src\Rates.Functions" -o $functionsPublishDirectory -c Release
Test-ExitCode $lastExitCode

# create artifact directory
if (Test-Path $artifactDirectory) {
	Remove-Item $artifactDirectory -Recurse -Force
}
New-Item -ItemType Directory -Path $artifactDirectory

# zip functions
Compress-Archive $functionsPublishDirectory\** "$artifactDirectory\Rates.Functions.$version.zip"
Test-ExitCode $lastExitCode

# zip front-end
Copy-Item "$PSScriptRoot\src\Rates.Functions\wwwroot" "$artifactDirectory\Rates.Web" -Recurse

Copy-Item "$PSScriptRoot\Deploy.ps1" $artifactDirectory
