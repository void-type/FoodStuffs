[CmdletBinding()]
param(
  [string] $Configuration = "Release"
)

./build.ps1 -Configuration "$Configuration" -SkipClient -SkipPublish
