[CmdletBinding()]
param(
  [string] $Configuration = "Debug"
)

./build.ps1 -Configuration $Configuration -SkipClient -SkipTest -SkipPublish
