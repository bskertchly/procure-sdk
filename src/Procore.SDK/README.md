# Procore SDK for .NET

[![NuGet Version](https://img.shields.io/nuget/v/Procore.SDK.svg)](https://www.nuget.org/packages/Procore.SDK/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Procore.SDK.svg)](https://www.nuget.org/packages/Procore.SDK/)
[![Build Status](https://github.com/procore/procore-sdk-dotnet/workflows/CI/badge.svg)](https://github.com/procore/procore-sdk-dotnet/actions)

Unified .NET SDK for Procore's construction management platform. Provides access to all Procore API resources through strongly-typed clients with automatic authentication, error handling, and retry logic.

## Features

- 🔐 **OAuth 2.0 with PKCE**: Secure authentication flow implementation
- 🚀 **Strongly-typed clients**: Full IntelliSense support with generated API clients
- 🛡️ **Resilience patterns**: Automatic retry, circuit breaker, and timeout handling
- 📊 **Comprehensive logging**: Structured logging with correlation tracking
- 🎯 **Type mapping**: Seamless conversion between domain models and API types
- 📦 **Dependency injection**: Built-in DI container integration
- 🔄 **Multi-targeting**: Supports .NET Standard 2.0 and .NET 8.0

## Quick Start

### Installation

```bash
dotnet add package Procore.SDK
```

### Basic Usage

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Procore.SDK.Extensions;

// Configure services
var services = new ServiceCollection();
services.AddProcoreSDK(options =>
{
    options.BaseAddress = "https://sandbox.procore.com";
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.RedirectUri = "your-redirect-uri";
});

var serviceProvider = services.BuildServiceProvider();

// Use the Core client
var coreClient = serviceProvider.GetRequiredService<ICoreClient>();

// Authenticate (this will open browser for OAuth flow)
await coreClient.AuthenticateAsync();

// Get companies
var companies = await coreClient.GetCompaniesAsync();
foreach (var company in companies)
{
    Console.WriteLine($"Company: {company.Name} (ID: {company.Id})");
}
```

### ASP.NET Core Integration

```csharp
using Procore.SDK.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Procore SDK to DI container
builder.Services.AddProcoreSDK(builder.Configuration.GetSection("Procore"));

var app = builder.Build();

// Use in controllers
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly ICoreClient _coreClient;
    
    public ProjectsController(ICoreClient coreClient)
    {
        _coreClient = coreClient;
    }
    
    [HttpGet("companies")]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _coreClient.GetCompaniesAsync();
        return Ok(companies);
    }
}
```

## Available Clients

This meta-package includes all Procore SDK clients:

- **Procore.SDK.Core** - Companies, users, documents, and configuration
- **Procore.SDK.ProjectManagement** - Projects, workflows, and tasks  
- **Procore.SDK.QualitySafety** - Inspections, observations, and incidents
- **Procore.SDK.ConstructionFinancials** - Contracts, budgets, and invoices
- **Procore.SDK.FieldProductivity** - Timecards, daily logs, and equipment
- **Procore.SDK.ResourceManagement** - Workforce and resource scheduling

## Configuration

```json
{
  "Procore": {
    "BaseAddress": "https://sandbox.procore.com",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret", 
    "RedirectUri": "http://localhost:8080/callback",
    "TokenStorage": "File", // Options: InMemory, File, ProtectedData
    "HttpClient": {
      "Timeout": "00:01:00",
      "MaxConnectionsPerServer": 10
    },
    "Resilience": {
      "RetryAttempts": 3,
      "CircuitBreakerThreshold": 5,
      "TimeoutSeconds": 30
    }
  }
}
```

## Authentication Flows

### Console Application
```csharp
// Authenticate with PKCE flow (opens browser)
await coreClient.AuthenticateAsync();
```

### Web Application
```csharp
// Handle OAuth callback in controller
[HttpGet("callback")]
public async Task<IActionResult> OAuthCallback(string code, string state)
{
    await coreClient.HandleCallbackAsync(code, state);
    return RedirectToAction("Index");
}
```

## Error Handling

The SDK includes comprehensive error handling with custom exceptions:

```csharp
try
{
    var companies = await coreClient.GetCompaniesAsync();
}
catch (ProcoreAuthenticationException ex)
{
    // Handle authentication errors
    _logger.LogError(ex, "Authentication failed");
}
catch (ProcoreApiException ex)
{
    // Handle API errors
    _logger.LogError(ex, "API call failed: {StatusCode}", ex.StatusCode);
}
catch (ProcoreException ex)
{
    // Handle general SDK errors
    _logger.LogError(ex, "SDK error occurred");
}
```

## Performance & Resilience

The SDK includes built-in resilience patterns:

- **Retry Policy**: Exponential backoff with jitter
- **Circuit Breaker**: Prevents cascading failures  
- **Timeout Handling**: Configurable request timeouts
- **Connection Pooling**: Optimized HTTP client usage

## Documentation

- [Authentication Guide](https://github.com/procore/procore-sdk-dotnet/blob/main/docs/authentication.md)
- [Configuration Guide](https://github.com/procore/procore-sdk-dotnet/blob/main/docs/configuration.md)
- [API Reference](https://github.com/procore/procore-sdk-dotnet/blob/main/docs/api-reference.md)
- [Sample Applications](https://github.com/procore/procore-sdk-dotnet/tree/main/samples)

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/procore/procore-sdk-dotnet/blob/main/CONTRIBUTING.md) for details.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/procore/procore-sdk-dotnet/blob/main/LICENSE) file for details.

## Support

- 📖 [Documentation](https://github.com/procore/procore-sdk-dotnet/wiki)
- 🐛 [Issue Tracker](https://github.com/procore/procore-sdk-dotnet/issues)
- 💬 [Discussions](https://github.com/procore/procore-sdk-dotnet/discussions)
- 📧 [Email Support](mailto:developers@procore.com)