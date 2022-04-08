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

- MANIPULATING RESOURCES & VALIDATING INPUT:

- WORKING WITH SERVICES & DEPENDENCY INJECTION:

- GETTING AQUAINTED WITH ENTITY FRAMEWORK CORE: