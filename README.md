This is simple application that demonstrates maintaining a database to track changes of a specified shop's listings on Etsy.

Web application built on the .NET Core 2.2 platform.  Solution developed on Windows OS but can run in MacOS|Linux|Unix as well.
Note: Application build and run has not been tested on MacOS|Linux|Unix but .NET Core is able to run in those environments.

## How to build and run application

Before you run the application or unit tests you will have to enter a valid Etsy Developer API Key.

Inventory Tracking Service - Enter the Etsy API Key in the designated area of json file.
{git project root}\listinglistenerApp\InventoryTrackingSvc\appsettings.json 

Unit\Integration Tests - Enter the API Key in the first line of the following text file.
{git project root}\listinglistenerApp\XUnitTestInventoryTracking\EtsyAPIKey.txt 


**Option 1** (Recommended)

Open solution with [Visual Studio Community (free)](https://visualstudio.microsoft.com/vs/community/)
Visual Studio Community installation typical comes with the .Net Core SDK

You will need to install [.NET Core 2.2] or higher (https://dotnet.microsoft.com/download) runtime.

1. Open the listinglistener.sln solution file.
2. Build application from within IDE.
3. Run application from within IDE.

**Option 2**

You will need to install [.NET Core 2.2] or higher (https://dotnet.microsoft.com/download) runtime and SDK.
The SDK will provide command line tools.

1. Using the shell navigate to the project root.
Run commands: 
2. dotnet build .\listinglistenerapp.sln
3. dotnet run --project .\listinglistenerapp.csproj --launch-profile InventoryTrackingSvc
4. dotnet run --project .\InventoryTrackingSvc.csproj --launch-profile ListingListenerApp

Open browser with the URL specified in the shell.

You can change the port in the launchSettings.json
{project root}\listinglistenerapp\Properties\launchSettings.json.

NOTE:If you are having issues running the application, you may need to delete the .vs folder in the project.
{git project root}\listinglistenerApp\.vs
This folder will be regenerated during the build.

## Using the application.
1. You can send a request via the Web UI.
2. Or you can directly invoke a web service request via shell.
e.g. Powershell
$url = "http://{baseUrl}/api/v1/sync/shops/"
$shopIds = (19099945,19099937,19120225) | ConvertTo-Json 
$inventoryChanges = Invoke-WebRequest -Uri $url -Method Post -ContentType "application/json" -Body $shopIds | ConvertFrom-Json
$inventoryChanges
