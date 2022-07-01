<h1 align="center">WebPrintManager</h1>
<p align="center">The most advanced open-source to build powerful thermal printing solution with no effort.</p>
<br />
<p align="center">
  <a href="https://raw.githubusercontent.com/fabrimaciel/webprintmanager/main/LICENSE">
    <img src="https://img.shields.io/github/license/fabrimaciel/webprintmanager" />
  </a>
  <a href="https://github.com/fabrimaciel/webprintmanager/issues">
    <img src="https://img.shields.io/github/issues/fabrimaciel/webprintmanager" />
  </a>
  ![Nuget](https://img.shields.io/nuget/v/webprintmanager)
  ![Nuget](https://img.shields.io/nuget/dt/webprintmanager)
</p>


## Print Commands from Blazor Apps

**WebPrintManager** is an solution for **Client-side Printing** scenarios **designed to be used in any Blazor Server & WebAssembly projects!**

By writing pure .NET C# code, **WebPrintManager** allows you to _easily send raw data, text and native commands_ as well to any printer installed or available at the client machine *without showing or displaying any print dialog box!*

## Features

### Raw Data Printing
Send any raw data & commands supported by the client printer like EPSON ESC/POS!

## Installing and Managing the Agent Windows Service

After building the application, the new Windows Service can be published using dotnet publish

````
dotnet publish .\src\WebPrintManager.Agent\WebPrintManager.Agent.csproj -c Release -oc:\webprintmanageragent
````

To control Windows Services, the sc command can be used. Creating a new Windows Service is done using sc create passing the name of the service and the binPath parameter referencing the executable. This command requires administrator rights:

````
sc create "WebPrintManager Agent" binPath= c:\webprintmanageragent\WebPrintManager.Agent.exe start= auto
````

The status of the service can be queried using the Services MMC, or with the command line sc query:

````
sc query "WebPrintManager Agent"
````

After the service is created, it is stopped and need to be started:

````
sc start "WebPrintManager Agent"
````

To stop and delete the service, the sc stop and sc delete commands can be used.