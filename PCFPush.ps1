cd SampleWebAPI
dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
cf create-service  p-mysql 100mb MySqlService
cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish