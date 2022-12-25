#Requires -Version 7.0
[CmdletBinding()]
param (
    [string] $Version,
    [string] $Suffix,
    [string] $Configuration = 'Release',
    [switch] $AutoGenerateSuffix,
    [switch] $PublicOnly,
    [switch] $BuildOnly
)

$apiKey = [System.Environment]::GetEnvironmentVariable('NUGET_API_KEY')
$sourceUrl = [System.Environment]::GetEnvironmentVariable('NUGET_SOURCE_URL')

if ( [string]::IsNullOrEmpty($apiKey))
{
    Write-Error 'NUGET_API_KEY is empty' -ErrorAction Stop
}
if ( [string]::IsNullOrEmpty($sourceUrl))
{
    Write-Error 'NUGET_SOURCE_URL is empty' -ErrorAction Stop
}

$buildDir = '.\.build'

if (!$PublicOnly)
{

    rm "$buildDir\*" -Force -Recurse

    # $version = Read-Host -Prompt 'Input version'

    dotnet clean -v m
    if ($Suffix)
    {
        dotnet build --configuration $Configuration -p:VersionSuffix = $Suffix
        dotnet pack --no-build --configuration $Configuration -p:VersionSuffix = $Suffix -o $buildDir
    }
    elseif($Version)
    {
        dotnet build --configuration $Configuration -p:Version = $Version
        dotnet pack --no-build --configuration $Configuration -p:Version = $Version -o $buildDir
    }
    elseif($AutoGenerateSuffix)
    {
        $suffix = "{0:x}" -f [long][datetime]::Now.ToString('HHmmss')
        dotnet build --configuration $Configuration --version-suffix $suffix
        dotnet pack --no-build --configuration $Configuration --version-suffix $suffix -o $buildDir
    }
    else
    {
        dotnet build --configuration $Configuration
        dotnet pack --no-build --configuration $Configuration -o $buildDir
    }
}

$path = "{0}\*.nupkg" -f $buildDir

ls $path | % {
    $file = $_.FullName

    Write-Host $file

    if (!$BuildOnly)
    {
        nuget push -Source $sourceUrl -ApiKey $apiKey "$file"
    }
}