$gameName = "VoidWalker"
$version = "v0.0.4"
$publishToItch = $False
$mainProject = "source/VoidWalker.Main"

##############################################################################################################

$ErrorActionPreference = 'Stop'

$platforms = @('Windows')#, 'Linux', 'MacOS')
$rids = @{
    'Windows' = 'win-x64';
    'Linux' = 'linux-x64';
    'MacOS' = 'osx-x64';
}

$publishDir = [IO.Path]::Combine($pwd, "publish")

# Ah, PowerShell, Ah.
# Seems everything is relative to the solution root, not the cwd (source)
$zipsPath = [IO.Path]::Combine("..", "*.zip")
remove-item $zipsPath

foreach ($platform in $platforms)
{
    $zipFile = "$gameName-$version-$platform.zip"

    if (Test-Path($publishDir))
    {
        Remove-Item $publishDir -Recurse
    }

    # Source: https://stackoverflow.com/questions/44074121/build-net-core-console-application-to-output-an-exe
    # Publish to an exe + dependencies. 40MB baseline.
    dotnet publish $mainProject -c Release -r $rids[$platform] -o $publishDir

    $command = ("chmod a+x " + [IO.Path]::Combine($publishDir, $gameName))
    if ($platform -eq 'Windows')
    {
        $command += ".exe"
    }
    #Invoke-Expression $command

    # Zip it up.
    if (Test-Path($zipFile))
    {
        Remove-Item $zipFile -Force
    }

    #chmod -R 755 $publishDir

    if ($publishToItch) {
        iex "butler push $publishDir deengames/${gameName}:$platform"
    } else {
        Add-Type -A 'System.IO.Compression.FileSystem'
        [IO.Compression.ZipFile]::CreateFromDirectory($publishDir, $zipFile);
        Write-Host "DONE! Zipped to $zipFile"
    }
}