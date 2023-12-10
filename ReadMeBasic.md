# DocPlanner API Readme

### Introduction

This document provides comprehensive instructions and details for the DocPlanner API - a .Net 8 Asp.Net Core Web API project, along with its associated xUnit test project. This solution is designed for booking doctor's appointments and managing available slots.

### Solution Components:

    -DocPlanner.API: A .Net 8 Asp.Net Core Web API project.
    -DocPlanner.Test: A .Net 8 xUnit test Project.

### Getting Started
### Prerequisites:

    -Latest version of Visual Studio.
    -.Net 8 Framework and necessary Nuget Packages.

### Running the REST API:

    -Download the solution from the Git repository .
    -Unzip the solution, if necessary.
    -Open the solution by double-clicking on DocPlanner.API.sln.
    -Build the solution, ensuring all Nuget Packages and frameworks are installed.
    -Set DocPlanner.API as the startup project.
    -Start the application from Visual Studio.

### Using the UI:

The DocPlanner.API runs Swagger UI in the browser, providing GET and POST functionalities. 

### GET Functionality

    -Click on the blue "GET" button.
    -Select "Try it out" in the top right corner of the blue box.
    -Enter a valid date (YYYYMMDD format) in the required field.
    -Click "Execute".
    -Review "Code" and "Response Body" for server responses.

### POST Functionality

    -Click on the green "POST" button.
    -Select "Try it out" in the top right corner of the green box.
    -Enter valid JSON data in the Request body text box.
    -Click "Execute".
    -Review "Code" and "Response Body" for server responses.

### Running the Unit Tests:

    -In Visual Studio, go to the "View" menu and select "Test Explorer".
    -Run all tests using the "Run all tests" button.
    -Check for passing tests indicated by green status.

### Observations:

    -GET requests return 200 status codes with JSON data for valid inputs and error messages for 400, 500 status codes.
    -POST requests return specific status codes and messages based on input validity and completeness.

### Design Patterns Observed:

The solution employs various design patterns including Repository Pattern, Dependency Injection, 
Service Layer Pattern, Middleware Pattern, MVC, Factory Pattern, Adapter Pattern, Singleton Pattern, 
Command Pattern, and DTO Pattern for efficient and structured code management.


### Suggested User Story:

Title: Implement a Slot Booking System for Doctor's Appointments

Objective: Create a RESTful API in C# to interface with a slot service for booking doctor's appointments.

Acceptance Criteria:

    -RESTful API in C#, compliant with Visual Studio solution standards.
    -A basic UI demonstrating API functionality.
    -Adherence to KISS principle, using appropriate design patterns.
    -Clean, solid code with robust error handling and unit testing.
    -Comprehensive documentation and git repository access.
    -Functionalities for viewing and booking slots.
    -Test projects demonstrating code robustness.
    -Avoidance of unnecessary complexities like user authentication.

Notes:

    -Strict adherence to provided API endpoints and data structures.
    -Avoid direct consumption of the slot service from the front end or adding unnecessary complexities.

This user story is intended to guide development within the Scrum framework, aiming for a functional and high-quality solution.
