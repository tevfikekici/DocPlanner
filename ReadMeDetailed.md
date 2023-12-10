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

    -Download the solution from the Git repository,
    -Unzip the solution, if necessary,
    -Make sure you have latest version of Visual Studio,
    -Open the solution by double-clicking on DocPlanner.API.sln,
    -Build the solution, ensuring all Nuget Packages and frameworks are installed,
    -Set DocPlanner.API as the startup project,
    -Start the application from Visual Studio.

### Using the UI:

The DocPlanner.API runs Swagger UI in the browser, providing GET and POST functionalities. 
When you run the DocPlanner.API you will see Swagger running on browser.

### GET Functionality

    -Click on the blue "GET" button.
    -Select "Try it out" in the top right corner of the blue box.
    -Enter valid date value as 4 digits for the year, 2 for the month and 2 for the day into the required field text box. Example: to request the current week, should be: 20231120 ,
    -Click "Execute".
    -Review "Code" and "Response Body" for server responses.

### POST Functionality

    -Click on the green "POST" button.
    -Select "Try it out" in the top right corner of the green box.
    -Enter valid Json data into the Request body text box which should look like:

        {
         "Start": Start timestamp (string "YYYY-MM-dd HH:mm:ss" ),
         "End": End timestamp (string "YYYY-MM-dd HH:mm:ss" ),
         "Comments": Additional instructions for the doctor (string),
         "Patient": {
             "Name": Patient Name (string),
             "SecondName": Patient SecondName (string),
             "Email": Patient Email (string),
             "Phone": Patient Phone (string)
           }
        }

    -Click "Execute".
    -Review "Code" and "Response Body" for server responses.

### Running the Unit Tests:

    -In Visual Studio, go to the "View" menu and select "Test Explorer".
    -Run all tests using the "Run all tests" button.
    -Check for passing tests indicated by green status, you should see tests below green as passing:
       - GetWeeklyAvailabilityAsync_ReturnsAvailability
	   - TakeSlot_ReturnsOkResult_WhenSlotBookingIsSuccessful
	   - TakeSlot_ReturnsBadRequest_WhenServiceReturnsError

### Observations:

  ####  GET request on service:
    Service returns:
     - 200 status code with Json data in Response body using valid parameter: 20231120 
     - 400, 500 etc. status codes with error messages.

   #### POST request on service:
    Service returns:
       - 400 status code with BadRequest - "Valid slot required" message in Response body using example json data explained as below:
    {
     "Start": Start timestamp (string "YYYY-MM-dd HH:mm:ss" ),
     "End": End timestamp (string "YYYY-MM-dd HH:mm:ss" ),
    "Comments": Additional instructions for the doctor (string),
    "Patient": {
         "Name": Patient Name (string),
         "SecondName": Patient SecondName (string),
         "Email": Patient Email (string),
         "Phone": Patient Phone (string)
       }
    }

    - 400 status code with error message for null, invalid and required json object fields
    - 200 status was not observed.

### Design Patterns Observed:

During the development it was intended to keep the solution as simple as possible to align with acceptance criteria with no abuse of design patterns, how ever below are observed design patterns in the solution,

    1. Repository Pattern: This isn't explicitly implemented in the provided code, but the structure of ISlotService and SlotService suggests an abstraction layer for data access operations, which is a key aspect of the Repository pattern.
    2. Dependency Injection (DI): The use of interfaces like ISlotService and their implementation in classes like SlotService, which are then injected into controllers like AvailabilityController, is a classic example of Dependency Injection. This is further reinforced by the service configuration in the Startup class where services are added to the DI container.
    3. Service Layer Pattern: SlotService acts as a service layer, encapsulating the business logic of the application. This separation of concerns ensures that the business logic is decoupled from the presentation layer (controllers).
    4. Middleware Pattern: The ExceptionMiddleware class is an implementation of the Middleware pattern, used for handling exceptions globally in the ASP.NET Core application. This pattern allows for a request pipeline where each middleware can perform operations before and after the next component in the pipeline.
    5. Model-View-Controller (MVC): Although not fully fleshed out in the code, the structure with controllers (AvailabilityController), and models (WeeklyAvailability, SlotBooking, etc.) suggests the use of the MVC pattern, which is a fundamental design pattern in ASP.NET Core applications.
    6. Factory Pattern: The WebApplication.CreateBuilder(args) and the setup in the builder. Services section indicate a form of the Factory pattern, where objects are created without exposing the instantiation logic to the client and the client uses the same common interface to create new types of objects.
    7. Adapter Pattern: The HttpClient is being used as an adapter to convert the interface of the external API into an interface expected by the application.
    8. Singleton Pattern: The usage of logging (e.g., ILogger<ExceptionMiddleware>) and configuration (HttpClient) typically employs the Singleton pattern, where a single instance is used throughout the application's lifetime.
    9. Command Pattern: The methods in ISlotService, which encapsulate all information needed to perform an action or trigger an event, thereby allowing for parameterization of clients with different requests.
    Data Transfer Object (DTO) Pattern: Classes like WeeklyAvailability, BusySlot, and SlotBooking are used as DTOs. They are simple objects used to transfer data across layers and boundaries of the application, particularly in network calls.

### SOLID Principles observed:

    1. Single Responsibility Principle (SRP): Each class in the code have a single responsibility. For instance, SlotService is responsible for handling operations related to slot bookings. This principle ensures that each class has only one reason to change, making the code more maintainable.
    2. Open/Closed Principle (OCP): By using interfaces like ISlotService, the code demonstrates adherence to the Open/Closed Principle. The system is open for extension (you can implement different versions of ISlotService) but closed for modification, as changes to SlotService do not require modifications in the classes that use it.
    3. Liskov Substitution Principle (LSP): Although not explicitly demonstrated due to the lack of class inheritance and polymorphism in the provided code, the use of interfaces hints at adherence to LSP, where derived classes must be substitutable for their base classes.
    4. Interface Segregation Principle (ISP): The ISlotService interface is segregated based on specific functionalities related to slot services, adhering to ISP by not forcing the implementing classes to depend on methods they do not use.
    5. Dependency Inversion Principle (DIP): The code demonstrates this principle by depending on abstractions (ISlotService interface) rather than concrete implementations. This is evident in the constructor injection used in AvailabilityController where it depends on ISlotService.


### OOP Concepts observed:

    1. Abstraction: Abstraction is used through the definition of the ISlotService interface, which provides an abstract layer over the implementation details of SlotService.
    2. Encapsulation: The code demonstrates encapsulation by keeping the data of the classes private and exposing only the necessary methods to interact with this data. For example, SlotService encapsulates its logic and data, exposing only relevant methods like GetWeeklyAvailabilityAsync.
    3. Inheritance: While explicit inheritance is not visible in the provided code (no class is extending another class), the use of interfaces can be considered a form of inheritance. It defines a contract that other classes can implement.
    4. Polymorphism: Through the use of interfaces (like ISlotService), the code implicitly uses polymorphism. Different classes can implement ISlotService, and the consuming code (like AvailabilityController) can use these implementations interchangeably.
    5. Dependency Injection: This is more of a design pattern but is crucial in OOP for reducing coupling between classes. The use of constructor injection in AvailabilityController for ISlotService is an example of dependency injection.


### Suggested User Story:
User Story: Doctor's Appointment Slot Booking System

Title: Implement a Slot Booking System for Doctor's Appointments

As a software developer,
I want to create a RESTful API in C# that interfaces with a slot service for booking doctor's appointments,
So that patients can view available slots on a weekly basis and book appointments easily without handling work periods directly.

#### Acceptance Criteria:

    1. RESTful API Creation:
         • The API should be developed in C#, following the latest Visual Studio solution standards.
         • It must interact with the slot service for handling appointment slots, abstracting the work period management from the front end.
    2. User Interface:
         • A basic UI (like Swagger or a simple custom interface) should be provided to demonstrate the API functionality.
         • The UI should not interact directly with the slot service but through the C# API.
    3. Code Quality and Standards:
         • The solution should adhere to the KISS principle, ensuring simplicity and maintainability.
         • Appropriate design patterns should be used without overcomplicating the solution.
         • The code must be clean, solid, and demonstrate good unit testing practices.
         • Error handling should be robust and clear, ensuring a smooth user experience.
    4. Documentation and Setup:
         • Comprehensive instructions and observations for running the application should be included.
         • The entire codebase should be stored in a publicly accessible git repository for evaluation.
    5. Functionality:
         • Users should be able to view available slots by week and book appointments by selecting a slot and filling in the required data.
         • Slot availability and booking should follow the format and endpoints provided (e.g., GET for availability, POST for booking).
    6. Testing and Evaluation:
        • The solution should include test projects that demonstrate the functionality and robustness of the code.
        • The application should be easily configurable and quick to run.
    7. Non-Requirements:
        • The UI is not a primary evaluation criterion; it’s only to prove the solution works.
        • Avoid unnecessary complexities such as user login/password handling or setting up a local database.

#### Notes:

    • The provided API endpoints and data structures (e.g., slot duration, work periods, facility details) must be adhered to.
    • Common mistakes like directly consuming the slot service from the front end or adding unnecessary complexity should be avoided.

This user story is crafted to guide the development process within the Scrum framework, focusing on delivering a functional, high-quality solution that meets the outlined requirements and expectations.
