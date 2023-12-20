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

## How to run test suite locally
Once you have made _Chirp!_ work locally, you can proceed to running tests. Before the tests can be run, Playwright needs to be properly installed. Here follows a guide to install Playwright correctly:
1. In the terminal, run the command `dotnet tool install --global Microsoft.Playwright.CLI`
2. Navigate to the directory `test\Chirp.Web.End2EndTests`
3. Run the command `playwright install`

Once you have installed Playwright, the tests are now ready to run. Here follows a guide to running our test suite locally (Make sure you have the local _Chirp!_ up and running):
1. In the terminal, navigate to the root directory (Chirp) of the project
2. Run the command `dotnet test`

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
