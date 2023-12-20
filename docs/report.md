---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group `5`
author:
  - "Jakob Arnfred Nielsen <arni@itu.dk>"
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

### Running all tests

You need to have made _Chirp!_ work locally, before proceeding to running the tests. Before the end-2-end (E2E) tests can be run, Playwright needs to be installed. Here is a guide to install Playwright:

1. In the terminal, run the command: `dotnet tool install --global Microsoft.Playwright.CLI`
2. Navigate to the directory: `/test/Chirp.Web.End2EndTests`
3. Run the command: `playwright install`

Once you have installed Playwright, the tests are now ready to run. Here follows a guide to running our test suite locally (Make sure you have the local _Chirp!_ up and running):

1. In the terminal, navigate to the root directory (/Chirp) of the project
2. Run the command: `dotnet test`
3. A browser window will open, and at some point you will be requested to login to GitHub.

### Running only unit & integration tests

If you do not want to run the E2E tests, they can be filtered out, allowing you to only run the unit and integration tests, with the following command:

`dotnet test --filter "TestCategory!=End2End"`

This command will produce the following warning:
_No test matches the given testcase filter 'TestCategory!=End2End'_

This is because we use both NUnit and xUnit for our tests. We use NUnit for the E2E tests and xUnit for the rest. NUnit correctly ignores the E2E test, but xUnit will not recognize these tests as they are not part of the xUnit test suite, resulting in the warning being produced.

### About our test suite

The test suite contains three different test projects:

1. Chirp.Infrastructure.Tests
2. Chirp.Web.End2EndTests
3. Chirp.Web.Tests

The Chirp.Infrastructure.Tests and Chirp.Web.Tests project contains unit-tests and integration tests for the Chirp.Infrastructure and Chirp.Web projects respectively.

The Chirp.Web.End2EndTests project tests the whole program, including UI and key functionality. This test project uses the NUnit test framework. The other projects use the xUnit testing tool.

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
