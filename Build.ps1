dotnet restore ./Orleans.Sagas/Orleans.Sagas.csproj --source https://dotnet.myget.org/F/orleans-prerelease/api/v3/index.json --source https://api.nuget.org/v3/index.json
dotnet publish ./Orleans.Sagas/Orleans.Sagas.csproj
dotnet pack ./Orleans.Sagas/Orleans.Sagas.csproj /p:PackageVersion=0.0.'$env:BUILD'-pre