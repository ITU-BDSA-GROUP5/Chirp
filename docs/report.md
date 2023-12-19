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
The following UML activity diagrams illustrate the GitHub actions workflows that are run when different criteria are met. This will be briefly described under the respective diagrams.

![UML activity diagram of the build and test workflow.](images/Build_test_release_and_deployment/build_and_test_workflow.png)

This workflow is run upon every push to main and pull request to main. It builds and tests to application in order to keep main void of faulty code (as **safety net**).

![UML activity diagram of the deployment workflow.](images/Build_test_release_and_deployment/deployment_workflow.png)

This workflow is run upon every push to main. Note the redundant "build" step. We do not need this since the "publish" step already builds the application. This redundancy was not noticed during development and has not been removed due to time constraints. The illustration describes the two jobs: "build" and "deploy". Jobs are normally run in parallel, however, these are run sequentially as we do not want to deploy before the application is successfully built.

![UML activity diagram of the release razor workflow.](images/Build_test_release_and_deployment/release_razor_workflow.png)

The Release Razor workflow is only run if a push to main contains a version tag. We've made use of a matrix strategy in order to automatically create multiple job runs that builds, publishes, zips and releases the application for their respective platform.

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
