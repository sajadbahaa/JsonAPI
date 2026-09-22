JsonAPI 🚀
A robust, production-ready sample web API project built with ASP.NET Core (.NET 10) and C# 14, demonstrating best practices for backend architecture, security, and maintainability.

📋 Table of Contents
Project Overview

Tech Stack

Prerequisites

Getting Started (Build & Run)

Configuration

Architecture & Dependency Injection

Authentication & Authorization

Middleware Pipeline

Logging & Diagnostics

CORS Policy

Swagger / API Exploration

Contributing

License

🔍 Project Overview
JsonAPI serves as a reference implementation for modern ASP.NET Core web services. Key features include:

JWT-Based Authentication & Advanced Authorization: Secure token-based authentication paired with custom policy handlers (UserOwnerOrAdmin).

Typed HTTP Clients: Integrated with custom delegating handlers (AuthHandler) to seamlessly inject tokens into outgoing external API requests.

Centralized Exception Handling: Custom middleware ensuring consistent, clean error responses.

Structured Logging: File-based diagnostic logging powered by Serilog.

Interactive API Documentation: Fully configured OpenAPI/Swagger UI supporting JWT Bearer authorization out of the box.

🛠️ Tech Stack
Runtime: .NET 10 SDK (TargetFramework: net10.0)

Language: C# 14

Hosting Model: ASP.NET Core minimal hosting model

Logging: Serilog (Serilog.AspNetCore, Serilog.Sinks.File)

Documentation: Swashbuckle (Swagger)

Security: Microsoft.IdentityModel.Tokens / JwtBearer

📦 Prerequisites
Ensure you have the following installed on your development machine:

.NET 10 SDK

Visual Studio 2026, VS Code, or your preferred IDE

PowerShell (optional, for CLI workflows)

🚀 Getting Started (Build & Run)
Clone the repository and run the application from the repository root:

Bash
# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the API project
dotnet run --project JsonAPI
Alternatively, open the solution file JsonAPI.slnx in Visual Studio or VS Code, set JsonAPI as the startup project, and hit F5.

💡 Note on Debug Output: When running in Debug configuration, the built application binaries and execution log files are located under JsonAPI/bin/Debug/net10.0/.

⚙️ Configuration
Application settings are managed via standard appsettings.json configurations and environment variables.

Key configuration sections include:

TokenSettings: Maps directly into token options for generation and validation.

ExternalService: Endpoint URLs and configuration consumed by HttpClientService.

⚠️ Security Warning: The repository contains a sample symmetric key ("THIS_IS_A_VERY_SECRET_KEY_123456") for demonstration purposes. Always replace this with a secure secret via environment variables or user-secrets in production environments.

🏗️ Architecture & Dependency Injection
Centralized service registrations are cleanly organized inside Program.cs. Key services include:

Domain Services (Scoped): UserService, PostService

Authentication Helpers (Singleton/Scoped): IAuth / AuthService, ITokenService / TokenService

Typed HTTP Infrastructure:

HttpClientService configured via AddHttpClient<T>().

AuthHandler: A custom DelegatingHandler automatically attaching authorization tokens to outgoing HTTP calls.

IHttpContextAccessor: For ambient HTTP context resolution across layers.

🔐 Authentication & Authorization
Authentication
Configured using JWT Bearer tokens (JwtBearerDefaults.AuthenticationScheme).

Validates issuer, audience, token lifetime, and the cryptographic signing key.

Authorization
Utilizes a custom authorization policy named UserOwnerOrAdmin.

Implemented via custom requirement types (UserOwnerOrAdminRequirement) and constraint handlers (UserOwnerOrAdminHandler).

Apply security constraints to controllers using standard [Authorize(Policy = "UserOwnerOrAdmin")] attributes.

🔄 Middleware Pipeline
The HTTP request pipeline execution order is strictly optimized:

ValidationExceptionMiddleware — Catches validation errors and unhandled exceptions, translating them into structured problem responses.

UseHttpsRedirection — Enforces secure transport channels.

CORS — Applies the registered cross-origin resource sharing policy.

Authentication — Identifies the caller via JWT validation.

Authorization — Evaluates user permissions against endpoint policies.

MapControllers — Dispatches requests to endpoint controllers.

📊 Logging & Diagnostics
Provider: Serilog handles application-wide logging.

Sink: Writes diagnostics directly to a file sink located at AppContext.BaseDirectory/JsonAPI.log (e.g., JsonAPI/bin/Debug/net10.0/JsonAPI.log).

Level: Defaults to Debug (customizable in Program.cs).

Lifecycle Events: HttpClientService explicitly injects ILogger<HttpClientService> to trace external request and response lifecycles.

🌐 CORS Policy
A designated CORS policy named UserApiCorsPolicy is registered to permit requests from specific trusted origins:

https://localhost:7061

http://localhost:5166

(Modify or expand allowed origins directly within Program.cs if your environment dictates distinct client URLs).

📖 Swagger / OpenAPI
Interactive API exploration and testing are enabled out-of-the-box when running in the Development environment.

Navigate to the Swagger UI root endpoint while running the app locally.

Click the Authorize button in the UI and input your token using the format:
Bearer {JWT_TOKEN}

🤝 Contributing
Contributions, feature additions, and bug fixes are welcome!

Fork the repository.

Create a feature branch (git checkout -b feature/amazing-feature).

Commit your changes (git commit -m 'Add some amazing feature').

Push to the branch (git origin push feature/amazing-feature).

Open a Pull Request.

Development Guidelines:
Keep Dependency Injection registrations centralized in Program.cs.

Leverage IConfiguration and the options pattern (IOptions<T>) for managing configurable parameters.
