---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group `5`
author:
  - "Jakob Arnfred <arni@itu.dk>"
  - "Johan Brandi <johbr@itu.dk>"
  - "Niklas Zeeberg Hessner Christensen <nizc@itu.dk>"
  - "Olivier-Baptiste Hansen <oliha@itu.dk>"
  - "Philip Guozhi Han Pedersen <phgp@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model.

![Illustration of the _Chirp!_ data model as UML class diagram.](docs/images/domain_model.png)

## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally
### Prerequisites
- .NET (>=7.0)
- dotnet ef-tools
- Docker
- Azure AD B2C Tenant
- Github OAuth App

### Azure AD B2C
_Chirp!_ uses an Azure AD B2C user flow for authentication. The `AzureADB2C` object in `appsettings.Development.json` must be configured according to [Step 2. of the documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/azure-ad-b2c?view=aspnetcore-8.0).

1. Add a user-secret from within the `src/Chirp.Web` folder, using the following command in your terminal:
```bash
dotnet user-secrets set "AzureADB2C:ClientSecret" "[CLIENT-SECRET]"
```
| Replacing [CLIENT-SECRET] with your client secret from Azure.

2. You must configure uour [Github OAuth App with your user flow on your Azure ADB2C client](https://learn.microsoft.com/en-us/azure/active-directory-b2c/identity-provider-github?pivots=b2c-user-flow).

### Local database
_Chirp!_ uses an MSSQL database, which can be run locally using Docker.

1. Set up the database using the following command from your terminal:
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=my}}Pass;word" \
   -p 1433:1433 --name azuresql --hostname azuresql \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest
```
| Password specifications can be found [here](https://learn.microsoft.com/en-us/sql/relational-databases/security/strong-passwords?view=sql-server-ver16). The given password is not of importance and is only used for the individual Docker instance.

2. Add a user-secret from within the `src/Chirp.Web` folder, using the following command in your terminal:
```bash
dotnet user-secrets set "ConnectionStrings:AZURE_SQL_CONNECTIONSTRING" "Server=localhost,1433;Initial Catalog=bdsagroup5-chirpdb;Persist Security Info=False;User ID=sa;Password=my}}Pass;word;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
```

3. The database is configured using the dotnet-ef tool by running the following command from within the `src/Chirp.Infrastructure` folder:
```bash
dotnet ef database update --startup-project "../Chirp.Web"
```

### Running _Chirp!_
To run _Chirp!_, use the following command from within the `src/Chirp.Web` folder, in your terminal:
```bash
dotnet run
```

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
