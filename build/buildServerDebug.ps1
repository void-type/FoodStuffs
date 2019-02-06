[CmdletBinding()]
param(
  [string] $Configuration = "Debug"
)

./build.ps1 -Configuration Debug -SkipClient -SkipTest -SkipPublish
