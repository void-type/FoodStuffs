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
  $Domain = 'foodstuffs.home',

  [Parameter()]
  [string]
  $PhysicalPath = 'G:\DeployedApps\apps\foodstuffs'
)

$appPool = New-WebAppPool -Name $Domain
$appPool.managedRuntimeVersion = ""
$appPool.StartMode = 'AlwaysRunning'
$appPool.ProcessModel.LoadUserProfile = $true
$appPool | Set-Item

New-WebSite -Name $Domain -Port 80 -HostHeader $Domain -PhysicalPath $PhysicalPath -ApplicationPool $appPool.Name

New-WebBinding -Name $Domain -IPAddress "*" -Port 443 -HostHeader $Domain -Protocol "https" -SslFlags 1
$httpsBinding = Get-WebBinding -Name $Domain -Port 443 -Protocol "https"
$httpsBinding.AddSslCertificate($CertificateThumbprint, "my")
