nuget pack .\ObjectsComparer.csproj

nuget.exe push -Source "CasNugetPackages" -ApiKey VSTS .\ObjectsComparer.1.0.4.nupkg