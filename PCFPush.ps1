cd SampleWebAPI
dotnet publish -f netcoreapp2.0 -r ubuntu.14.04-x64
cf push -f manifest.yml -p bin/Debug/netcoreapp2.0/ubuntu.14.04-x64/publish