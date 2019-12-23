# Overview

This is a sample Windows service using .NET Core 3.1. The code is based off of the [example on this page](https://csharp.christiannagel.com/2019/10/15/windowsservice/) with some minor modifications.

## Creating the service

```bash
dotnet new sln -o WindowsService
cd WindowsService/
dotnet new worker -n Main -o Main
dotnet new xunit -n Tests -o Tests -lang f#
dotnet sln add Tests/Tests.fsproj Main/Main.csproj
dotnet add Tests/Tests.fsproj reference Main/Main.csproj
dotnet add Main/Main.csproj \
    package Microsoft.Extensions.Hosting.WindowsServices --version 3.1.0
```

## publish it as a single executable

This is being published using the `win7-x86` RID in order to publish on a 32-bit Windows 7 machine. (not recommended)

```bash
dotnet publish -r win7-x86 \
    -c Release -o /c/apps/testapps/workertest \
    -p:PublishSingleFile=True \
    -p:PublishTrimmed=True \
    -p:UseAppHost=True
```
