# =================================== #
#                                     #
#   This script aims to assist in     #
# compiling all projects and creating #
# standardized folders in accordance  #
#           with SPT version          #
#       publishing regulations.       #
#                                     #
# =================================== #

# Clear the console window
Clear-Host

# Define solutions and publishing directories
$csproj = "..\..\SPT\SPT.csproj"
$outputDirectory = "..\..\Publish"

# List of target platforms
$platforms = @(
    "win-x86", "win-x64", "win-arm64",
    "osx-x64", "osx-arm64",
    "linux-x64", "linux-arm", "linux-arm64"
)

# Function to publish a project for a given platform
function Publish-Project($projectPath, $platform) {
    Write-Host "Publishing $projectPath for $platform..."
    dotnet publish $projectPath -c Release -r $platform --output "$outputDirectory\SPT-$platform-v0.0.0.0"
    Write-Host "Publishing to $platform completed."
}

# Delete existing directories
if (Test-Path $outputDirectory -PathType Container) {
    Remove-Item -Path $outputDirectory -Recurse -Force
    Write-Host "Existing directory deleted."
}

# Publish for all platforms
Write-Host "Publishing to all platforms..."
foreach ($platform in $platforms) {
    Publish-Project $csproj $platform
    Write-Host "Next..."
}

Write-Host "All publishing processes have been completed."