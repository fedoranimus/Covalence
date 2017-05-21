# Covalence

Primary Repo for Covalence

## Getting Started

To run in development mode, set an environment variable

* If you’re using PowerShell in Windows, execute $Env:ASPNETCORE_ENVIRONMENT = "Development"
* If you’re using cmd.exe in Windows, execute setx ASPNETCORE_ENVIRONMENT "Development", and then restart your command prompt to make the change take effect
* If you’re using Mac/Linux, execute export ASPNETCORE_ENVIRONMENT=Development

If you get the error `Cannot find module './wwwroot/dist/vendor-manifest.json'`, run `npm run config`.

Once these are complete, run `dotnet run`