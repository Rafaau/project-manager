# Project Manager

This repository contains the source code for **Project Manager**, a web application built using the **Clean Architecture** (DDD) by **Steve Smith** pattern, **ASP.NET**, and **Blazor**.
The main goal of this project is to familiarize with the **Domain-Driven Design** and apply various interesting solutions within the scope of web development


## Architecture

The project structure is organized into the following layers:
- Core
- Infrastructure
- SharedKernel
- Web
- Tests

## Features
The **PM** application offers several simple functionalities, including:

- Creating channels for project members
- Chat communication
- Kanban Board for task management
- Calendar with appointments
- Private messaging
- Notifications

## Testing
The project includes various testing approaches:

- Unit tests to ensure the proper functioning of individual components
- Integration tests based on **TestContainer** (Docker) and a mocked API Server to verify the correct interaction between components and external dependencies
- E2E tests based on Selenium to validate the application's behavior from the user's perspective

## Screenshots

![alt text](https://imgur.com/qWl4hvB.jpg)
![alt text](https://imgur.com/0j76Xbs.jpg)
![alt text](https://imgur.com/oeoJJZC.jpg)
![alt text](https://imgur.com/Avo6NFQ.jpg)