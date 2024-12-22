dotnet sonarscanner begin /k:"ffsti-library" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="4b4772d91134033d02c97ea0a51b1a70a3d9b365"
dotnet build
dotnet sonarscanner end /d:sonar.login="4b4772d91134033d02c97ea0a51b1a70a3d9b365"