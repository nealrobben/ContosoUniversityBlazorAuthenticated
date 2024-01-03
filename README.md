# Contoso University Blazor - Authenticated
A fork of my ContosoUniversityBlazorExtended project meant for further experimentation with Authentication/Authorization.

This project is based off the [Contoso University](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-8.0) example from Microsoft.


It has been adapted to Clean Architecture based off the [Clean Architecture solution template](https://github.com/jasontaylordev/CleanArchitecture) from Jason Taylor as an API using CQRS with a Blazor WASM front-end.


The solution contains the following projects:

| Project					| Are           |
| --------------------------|---------------| 
| Application				| Application layer containing the use-cases for the business logic | 
| Domain      				| Domain layer containing the entities      | 
| Infrastructure 			| Infrastructure layer containing implementations for the interfaces from the application layer & persistence with EF Core      | 
| WebUI.Client 				| Blazor WASM client      | 
| WebUI.Server 				| API in ASP.Net Core       | 
| WebUI.Client.Test 		| Unit tests for the WebUI.Client project      | 
| WebUI.Integration.Tests 	| Integration tests      | 

To create the database, open the project in Visual Studio and go to the Package Manager Console window.
In there, make sure <strong>WebUI.Server</strong> is selected as the default project and run the following command to apply EF migrations to create/update the database:

```Powershell
Update-Database
```



[![Master build](https://github.com/nealrobben/ContosoUniversityBlazorAuthenticated/actions/workflows/Master%20build.yml/badge.svg)](https://github.com/nealrobben/ContosoUniversityBlazorAuthenticated/actions/workflows/Master%20build.yml)


