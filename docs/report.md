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

## Team work

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others
