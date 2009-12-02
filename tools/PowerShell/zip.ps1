
#usage: new-zip c:\demo\myzip.zip
function New-Zip
{
	param([string]$zipfilename)
	set-content $zipfilename ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
	(dir $zipfilename).IsReadOnly = $false
}

#usage: dir c:\demo\files\*.* -Recurse | add-Zip c:\demo\myzip.zip
function Add-Zip
{
    param([string]$zipfilename)
 
    if(-not (test-path($zipfilename)))
    {
        set-content $zipfilename ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
        (dir $zipfilename).IsReadOnly = $false
    }
	
	$actualFile = "$(get-location)\$zipfilename"
	$actualFile = $actualFile.Replace('\.\', '\');
	echo "actualFile path = $actualFile"
    
    $shellApplication = new-object -com shell.application
    $zipPackage = $shellApplication.NameSpace($actualFile)
	
	if(!$zipPackage)
	{
		throw "zip package not created - attempted to create with file path [$actualFile]. Make sure it's ***fully qualified***."
	}
    
    foreach($file in $input) 
    {
		if(Test-Path $file)
		{
            $zipPackage.CopyHere($file.FullName)
            Start-sleep -milliseconds 1500
		}
		else
		{
			throw "File not found [$file]"
		}
    }
}

#usage: Get-Zip c:\demo\myzip.zip
function Get-Zip
{
	param([string]$zipfilename)
	if(test-path($zipfilename))
	{
		$shellApplication = new-object -com shell.application
		$zipPackage = $shellApplication.NameSpace($zipfilename)
		$zipPackage.Items() | Select Path
	}
}

#usage: extract-zip c:\demo\myzip.zip c:\demo\destination
function Extract-Zip
{
	param([string]$zipfilename, [string] $destination)

	if(test-path($zipfilename))
	{	
		$shellApplication = new-object -com shell.application
		$zipPackage = $shellApplication.NameSpace($zipfilename)
		$destinationFolder = $shellApplication.NameSpace($destination)
		$destinationFolder.CopyHere($zipPackage.Items())
	}
}
