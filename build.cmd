
set version=%1
set key=%2

cd %~dp0

dotnet build magic.lambda.slots/magic.lambda.slots.csproj --configuration Release --source https://api.nuget.org/v3/index.json
dotnet nuget push magic.lambda.slots/bin/Release/magic.lambda.slots.%version%.nupkg -k %key% -s https://api.nuget.org/v3/index.json
