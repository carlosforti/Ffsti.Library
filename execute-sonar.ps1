dotnet sonarscanner begin /k:"ffsti-library" /d:sonar.login="088616ced24e2e09f0b6f55641da95c58775d656"
dotnet build
dotnet sonarscanner end /d:sonar.login="088616ced24e2e09f0b6f55641da95c58775d656"