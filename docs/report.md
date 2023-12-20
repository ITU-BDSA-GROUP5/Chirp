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

<!-- UML component diagram shows components, provided and required interfaces, ports, and relationships between them. -->

The following component diagram shows how the different high level components communicate with each other.

![Client-Server communication component diagram](/docs/images/architecture-of-deployed-app/component-communication.png)

The client communicates with the web server using HTTP requests. The server itself uses HTTP requests to get information about the user from the GitHub API. Furthermore, the server has a connection to the SQL Server which contains the database.

The following deployment diagram shows the artifacts of the system and where they are deployed.

![Deployment diagram](/docs/images/architecture-of-deployed-app/deployment-diagram.png)

Each client computer uses a browser, which uses HTTP requests, to access the website. The Chirp.Web project executable is deployed on an Azure App Server which hosts it, and has a connection to an Azure SQL Server. The SQL Server contains a database with all the persistent data of the "Chirp!" application.

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
