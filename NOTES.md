- GETTING AQUAINTED:
  - ASP.NET Core 6.0 & Entity Framework Core 6.0 & .NET 6.0 & Visual Studio 2022. With C# 10.
  - ASP.NET Core: A cross-platform, high-performance, open-source framework for building modern, cloud-enabled, Internet-connected applications.
  - CLI:
  ```javascript
    dotnet run 
  ```
  - New language feature. Top-level statements. e.g.: Program.cs.
  - IApplicationBuilder:
  ```csharp
     WebApplication.CreateBuilder(args);
  ```
  - ASP.NET Core Request Pipeline & Middleware.
    - Middleware: Software components that are assemblies into an application pipeline to handle requests and responses.
    - The order matters. Middleware can determine whether to pass the request along.
  - IWebHostEnvironment, IHostEnvironment.
  - Summary:
    - Program class is the starting point of the application. (Generated Main method is responsible for configuring and running the application.)
    - After building the Web application via WebApplicationBuilder, configure the request pipeline by adding middleware to it.
    - Potentially, scope middleware to different environments.

- CREATING THE API & RETURNING RESOURCES:
  - Clarifying the MVC Pattern:
    - Model-View-Controller. AN architectural software pattern for implementing user interfaces.
    - Loose coupling , seperation of concerns, testability, & reuse.
    - Not a full system/application pattern. Lives near the presentation layer.
  - Returning Resources:
    - Routing matches a request URI to an action on a controller.
      - Endpoint routing:
        - UseRouting() marks the position in the middleware pipeline where a routing decision is made.
        - UseEndpoints() marks the position in the middleware pipeline where the selected endpoint is executed.
        - Convention-based versus attribute-based. APIs *should* use attribute-based routing.
      ```csharp
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { app.MapControllers() });
      ```
    - NOTE: Shortcut: Calling MapControllers() on the WebApplication object directly will use attribute-based routing.
    - Attribute-based routing:
      - Use attributes [Route()], [HttpGet()] at controller and action level.
      - Combined with a URI template, requests are matched to controller actions.
      - For all common HTTP methods, a matching attribute exists.
      - [Route()] doesn't map to a HTTP method. Use it at a controller level to provide a template that will prefix all templates defined at an action level.
  - Interacting with an API:
    - Using Postman.
    - The importance of HTTP Status codes: Status codes tell the consumer of the API can inspect.
      - Did the request work as expected? And who is responsible for a failed request?
      - Do not send a 200 when something's wrong. Don't send a 500 when the client makes a mistake.
        - Level 100: Information. Typically not used by APIs.
        - Level 200: Success. e.g.: 200 OK. 201 Created. 204 No content.
        - Level 300: Redirection. Most APIs have no need for this.
        - Level 400: Client error. e.g.: 400 Bad request. 401: Unathorized. 403: Forbidden. 404: Not found. 409: Conflict.
        - Level 500: Server error. e.g.: 500 Internal server error.
  - Context Negotiation:
    - The process of selecting the best representation for a given response when there are multiple representations available.
    - Formatters & Content Negotation:
      - The media type(s) are passed through via the Accept header of the request:
        - application/json
        - application/xml
      - Supported via Output formatters.
      - Input formatter: Media type: Content-Type header.
      - Support is implemented by ObjectResult. Action result methods derive from it.
      - Rule: First formatter is the list, either input or outout, is the default.
  - Download file:
    ```csharp
      builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
    ```
  - Summary:
    - Model: Application data logic. View: Display data. Controller. Interaction between View & Model.
    - Routing matches a request URI to an action on a controller.
    - Attribute-based routing is advised for APIs.
    - Content negotiation is the process of selecting the best representation for a given response when there are multiple representations available.
    - Use the File() method on the ControllerBase to retutn files. And set the correct media type wwith the response.

- MANIPULATING RESOURCES & VALIDATING INPUT:
  - Manipulating Resources:
  - Passing Data To The API:
    - Data can be passed to an API by various means. 
    - Binding source attributes tell the model bnding engine where to find the binding source.
        Use binding source attributes to explicitly state where the action parameter should be bound from.
      ```csharp
        public ActionResult<DTO> Get([FromRoute()] int id]);
      ```
    - By default ASP.NET Core attempts to use the complex object model binder.
    - The [ApiController()] attribute changes the rules to beter fit APIs.
    - [FromBody()] Inferred for complex types.
    - [FromForm()] Inferred for action parameters of type IFormFile & IFormFileCollection.
    - [FromQuery()] Inferred for any other action parameters.
    - [FromRoute()] Inferred for any action parameter name matching a parameter in the route template.
    - [FromHeader()], [FromService()].
    - Creating Resources:
      -  We get the following snippit for free with [ApiController()]:
        ```csharp
          if (!ModelState.IsValid)
          {
            return BadRequest();
          }
        ```
      - Validation alternatives: This built-in approach is acceptable for simple use cases.
        - For complex rules, consider FluentVaidation.
    - Updating Resources:
      - Full versus partial updates. PUT for full.
      - Partially updating a resource: Json PATCH (RFC 6902.)
        - Describes a document structure for expressing a sequence of operations to a JSON document.
        - Array of operations. "Replace operation." Microsoft.AspNetCore.JsonPatch. Note the dependencies.
    - Deleting Resources:
  - Validating Input:
  - Summary:
    - Use POST for creating a resource. 201 Created. Header: Content-Type.
    - Validation: Data annotations & ModelState.
    - Use PUT for full updates & PATCH for partial updates. JsonPatch standard. - 204 No Content. (or 200 Ok.)
    - DELETE is for deleting resources. 204: No Content.

- WORKING WITH SERVICES & DEPENDENCY INJECTION:
  - Inversion of Control & Dependency Injection:
    - Class implementation has to change when a dependency changes. Difficult to test.
    - Class manages the lifetime of the dependency. This is tight coupling.
    - IoC: Delegates the function of selecting a concrete implementation type for a class's dependencies to an external component.
    - DI: A specialization of the IoC pattern which uses an object, the container, to initialize objects and provide the required dependencies to the object.
    - Services are registered on the container.
    - The container becomes responsible for providing instances when needed: It manages the service lifetime.
    - Interface: Not a concrete implementation. Class is decoupled from the concrete type.
    - Dependencies can be easily replaced. And with micking, the class becomes easier to test.
    - Dependency injection is built into ASP.NET Core. Register services on the built-in container in your Program class.
    - Use constructor injection when ever possible.
  - Logging in ASP.NET Core:
    ```csharp
      builder.Logging.ClearProviders();
    ```
    - Serilog:
    ```javascript
      install-package serilog.sinks.file
      install-package serilog.sinks.console
    ```
  - Creating & using custom services:
  - Working with configuration files & scoping them to environments:
  - Summary:
    - Dependency Injection: Specialization of IoC. Loose coupling. Less code changes. Better testability.
    - Custom services are registered on the built-in container.
      - Transient.
      - Scoped.
      - Singleton.
    - Use configuration files for consiguration data, scoped to a specific environment.

- GETTING AQUAINTED WITH ENTITY FRAMEWORK CORE:
  - ORM. EF Core. Migrations. Seeding.
  - Object-relational Mapping, ORM, is a technique that lets you query and mianipulate data from a database using an object-oriented paradigm.
  - Problems:
    - Relational models and object models do not work very well together.
    - Tabular database format versus an interconnected object graph.
  - Solution:
    - An ORM provides the library that implements the object-relational mapping technique.
    - It takes care of mapping between that tabular format and the object graph.
  - EF Core supports a variety of DBs, even non-relational.
  - Code-first or database-first approach.
  - DbContext.
    ```csharp
      protected override void OnConfiguring(DbContextOptionsBuilder builder)
      {
        builder.UseSqlite("ConnectionString");
        base.OnConfiguring(builder);
      }
    ```
  - Migrations:
    ```javascript
      add-migration INIT
      update-database
    ```
  - Safe location, environment variables, for "production" ConnectionStrings. NOTE: Use system variables.
  - Summary:
    - Object-Relational Mapping (ORM) is a technique that lets you query and manipulate data from a database using an object-oriented paradigm.
    - Seperate entity models from the outer-facing DTOs.
      - Use conventions or annotations for keys, required fields, etc...
    - The DbContext represents a session with the database and can be used to query and save instances of entities.
    - Migrations allow you to provide code to change the database from one verion to another.
    - Seeding the database is providing it with data to start woth. HasData() method is used when configuring the model.
    - Use environment variables for safer storage of sensitive data.

- USING EF CORE IN CONTROLLERS:
- 