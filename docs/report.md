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
![Domain Model as Class Diagram](/docs/images/domain_model_diagram.png)

The Author class represents a user in Chirp!.
A Cheep represents the messages shared on the Chirp! platform.
A Cheep is written by an Author, and an Author can write many Cheeps.
An Author can follow, as well as be followed, by another Author.
An Author can like a Cheep. A Cheep can be liked by many Authors.

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

The following user activity diagrams are for typical user journeys in our "Chirp!" application.

The figure below illustrates a user journey for an **unauthenticated** user of the application.

![Unauthenticated User activity diagrams](/docs/images/user-activity-diagrams/unauthenticated-user-activity-diagram.png)

The diagram below illustrates a user journey for an **authenticated** user of the application.

![Authenticated User activity diagrams](/docs/images/user-activity-diagrams/authenticated-user-activity-diagram.png)

The next diagram illustrates a user journey where a user wants to exercise their GDPR rights.

![Authenticated user downloads their data and deletes themselves](/docs/images/user-activity-diagrams/)

## Sequence of functionality/calls trough _Chirp!_
![Sequence of functionality/calls through chirp!\label{functionsequence}](docs/images/sequence_of_functionality.png)
The figure \ref{functionsequence} illustrates the sequence of events, from a user requests the root of __Chirp!__ to a page is rendered and returned.

# Process

## Build, test, release, and deployment
The following UML activity diagrams illustrate the GitHub actions workflows that are run when different criteria are met. This will be briefly described under the respective diagrams.

![UML activity diagram of the build and test workflow.\label{build_test_workflow}](images/Build_test_release_and_deployment/build_and_test_workflow.png)

The workflow on figure \ref{build_test_workflow} is run upon every push to main and pull request to main. It builds and tests to application in order to keep main void of faulty code (as a **safety net**).

![UML activity diagram of the deployment workflow.\label{deployment_workflow}](images/Build_test_release_and_deployment/deployment_workflow.png)

The deployment workflow illustrated on figure \ref{deployment_workflow} is run upon every push to main. Note the redundant "build" step. We do not need this since the "publish" step already builds the application. This redundancy was not noticed during development and has not been removed due to time constraints. The illustration describes the two jobs: "build" and "deploy". Jobs are normally run in parallel, however, these are run sequentially as we do not want to deploy before the application is successfully built.

![UML activity diagram of the release razor workflow.\label{release_workflow}](images/Build_test_release_and_deployment/release_razor_workflow.png)

The release workflow illustrated on figure \ref{release_workflow} only run if a push to main contains a version tag. We've made use of a matrix strategy in order to automatically create multiple parallel job runs that builds, publishes, zips and releases the application for their respective platform.

## Team work

No functionality is incomplete. Planned features we did not get to implement include:

* A character counter when writing cheeps
* Adding liked cheeps to about me page (as well as the "Download my data" json data)

To get an overview of our group work consult \ref{groupwork}.

After an issue has been created. We assign responsibility for it to one or more persons.

We work alone or with pair programming iteratively in research and implement steps until the issue is handled.

Then we create a pull request and notify group members who are not involved. They strive to review within 24 hours. If the request is approved we merge it into the Main branch. Otherwise we evaluate whether or not to continue working on the issue or close it with a comment.

Ideally after merging a tag is applied which causes the main branch to be redeployed.

[Group work process illustration\label{groupwork}](/docs/images/group-work-activity-diagram.png)

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

Our Chirp! application is licensed under [Creative Commons Attribution Share Alike 4.0 International](https://github.com/ITU-BDSA23-GROUP5/Chirp/blob/main/LICENSE).

This permits:
* Commercial use
* Modification
* Distribution
* Private use

With the following limitations:
* Liability
* Trademark use
* Patent use
* Warranty

Under the following conditions:
* License and copyright notice
* Changes are stated
* The license is kept


All dependencies in src use either the MIT License (e.g. Microsoft.EntityFrameworkCore.design, Microsoft.EntityFrameworkCore.SqlServer) or the Apache License 2.0 (e.g. FluentValidation), both of which are permissive.

## LLMs, ChatGPT, CoPilot, and others
We have used the following set of LLMs in some manner during the project.

We have used GitHub Copilot in the development process of the project. Copilot has mainly been utilized for generation of small code samples. This was done for a small amount of simple and repetitive methods and functions in the code. In these cases, the generated code was always heavily modified. 
GitHub Copilot's chat functionality within the workspace has also been used for proof of concept. The response helped us to get an idea of, how a certain functionality might be implemented. This would then lead to an inspired but manual implementation of that functionality.

We have also used ChatGPT in some rare cases during the project. This was mainly used for researching purposes to get details about a concept or receive an explanation of some code examples from the internet. The help from ChatGPT primarily helped to faster gain an understanding of a subject, before implementing a feature.

The usage of LLMs did not have a substantial effect on the development speed. In some situations, the LLMs improved our understanding of the given issue, and sped up the research phase. In other situations, the response was misleading or imprecise, which led to more refactoring of the written code.