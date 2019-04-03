param(
    [string[]] $Version = 'latest',
    [switch] $Publish,
    [pscredential] $Credential
)

begin {
    $ErrorActionPreference = 'Stop'

    function exec {
        param([scriptblock] $Command)

        & $Command
        if ($LASTEXITCODE -ne 0) {
            throw "Command failed with exit code $LASTEXITCODE"
        }
    }
}
process {

    if ($Credential) {
        $dockerUsername = $Credential.UserName
        $dockerPassword = $Credential.GetNetworkCredential().Password
        exec { docker login -u $dockerUsername -p $dockerPassword }
    }


    $Version | Select-Object -First 1 | ForEach-Object {
        $env:TAG = $_
        exec { docker-compose build --pull }
    }
    $Version | Select-Object -Skip 1 | ForEach-Object {
        $env:TAG = $_
        exec { docker-compose build }
    }
    if ($Publish) {
        $Version | ForEach-Object {
            $env:TAG = $_
            exec { docker-compose push }
        }
    }
}