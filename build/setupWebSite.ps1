#Requires -RunAsAdministrator
#Requires -Modules WebAdministration

[CmdletBinding()]
param (
  # Add your certificate to the Local Machine store along with all intermediate and CA certs in the chain. Then put the thumbprint here.
  # CA needs to be in Trusted Root Certification Authorities for browsers to trust the chain.
  # ls Cert:\LocalMachine\My\ | select Thumbprint, Subject | ft -AutoSize
  [Parameter(Mandatory = $true)]
  [string]
  $CertificateThumbprint,

  [Parameter()]
  [string]
  $Domain,

  [Parameter()]
  [string]
  $PhysicalPath = 'G:\DeployedApps\apps\foodstuffs'
)

$PhysicalPath = (Resolve-Path $PhysicalPath).Path

if ([string]::IsNullOrWhiteSpace($Domain)) {
  $baseUrl = (Get-Content "$PhysicalPath/appSettings.*.json" | ConvertFrom-Json).BaseUrl
  $Domain = (($baseUrl -split '://')[1] -split { $_ -in '/', '?' })[0]
  Write-Host "No domain provided, found and using '$Domain' from appSettings."
}

$appPool = New-WebAppPool -Name $Domain
$appPool.managedRuntimeVersion = ""
$appPool.StartMode = 'AlwaysRunning'
$appPool.ProcessModel.LoadUserProfile = $true
$appPool | Set-Item

New-WebSite -Name $Domain -Port 80 -HostHeader $Domain -PhysicalPath $PhysicalPath -ApplicationPool $appPool.Name

New-WebBinding -Name $Domain -IPAddress "*" -Port 443 -HostHeader $Domain -Protocol "https" -SslFlags 1
$httpsBinding = Get-WebBinding -Name $Domain -Port 443 -Protocol "https"
$httpsBinding.AddSslCertificate($CertificateThumbprint, "my")

$envName = ((Get-Item "$PhysicalPath/appSettings.*.json")[0].Name -split '\.')[1]
Add-WebConfigurationProperty -pspath 'MACHINE/WEBROOT/APPHOST' -location $Domain -filter "system.webServer/aspNetCore/environmentVariables" -name "." -value @{name='ASPNETCORE_ENVIRONMENT';value=$envName}
Add-WebConfigurationLock -pspath 'MACHINE/WEBROOT/APPHOST' -location $Domain -filter "system.webServer/aspNetCore/environmentVariables/environmentVariable[@name='ASPNETCORE_ENVIRONMENT' and @value=$envName]" -type general
