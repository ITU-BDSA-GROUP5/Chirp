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
![Domain Model as Class Diagram](/docs/images/domain_model_diagram.png)

The Author class represents a user in Chirp!.
A Cheep represents the messages shared on the Chirp! platform.
A Cheep is written by an Author, and an Author can write many Cheeps.
An Author can follow, as well as be followed, by another Author.
An Author can like a Cheep. A Cheep can be liked by many Authors.

## Architecture — In the small

## Architecture of deployed application

## User activities

The following user activity diagrams are for typical user journeys in our "Chirp!" application.

The figure below illustrates a user journey for an **unauthenticated** user of the application.

![Unauthenticated User activity diagrams](/docs/images/user-activity-diagrams/unauthenticated-user-activity-diagram.png)

The diagram below illustrates a user journey for an **authenticated** user of the application.

![Authenticated User activity diagrams](/docs/images/user-activity-diagrams/authenticated-user-activity-diagram.png)

The next diagram illustrates a user journey where a user wants to exercise their GDPR rights.

![Authenticated user downloads their data and deletes themselves](/docs/images/user-activity-diagrams/)

## Sequence of functionality/calls trough _Chirp!_

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

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
We have used the following set of LLMs in some manner during the project.

We have used GitHub Copilot in the development process of the project. Copilot has mainly been utilized for generation of small code samples. This was done for a small amount of simple and repetitive methods and functions in the code. In these cases, the generated code was always heavily modified. 
GitHub Copilot's chat functionality within the workspace has also been used for proof of concept. The response helped us to get an idea of, how a certain functionality might be implemented. This would then lead to an inspired but manual implementation of that functionality.

We have also used ChatGPT in some rare cases during the project. This was mainly used for researching purposes to get details about a concept or receive an explanation of some code examples from the internet. The help from ChatGPT primarily helped to faster gain an understanding of a subject, before implementing a feature.

The usage of LLMs did not have a substantial effect on the development speed. In some situations, the LLMs improved our understanding of the given issue, and sped up the research phase. In other situations, the response was misleading or imprecise, which led to more refactoring of the written code.