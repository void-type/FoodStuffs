#Requires -RunAsAdministrator

# Use this script and directions to setup IIS to run .NET websites.

# Install SQL Server Developer and SSMS by hand using defaults or desired settings.
# Allow SQL Firewall port if needed externally
# netsh advfirewall firewall add rule name = SQLPort dir = in protocol = tcp action = allow localport = 1433 remoteip = localsubnet profile = PRIVATE

# Install the .NET hosting bundle by hand https://dotnet.microsoft.com/en-us/download/dotnet

# Setup IIS Web Server
$features = @(
  'IIS-ManagementConsole'
  'IIS-StaticContent'
  'IIS-DefaultDocument'
  'IIS-DirectoryBrowsing'
  'IIS-HttpErrors'
  'IIS-StaticContent'
  'IIS-HttpLogging'
  'IIS-HttpCompressionStatic'
  'IIS-RequestFiltering'
)

Enable-WindowsOptionalFeature -Online -All -FeatureName $features -LogLevel Errors
