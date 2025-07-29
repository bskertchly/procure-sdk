# Procore SDK Shared Infrastructure

[![NuGet Version](https://img.shields.io/nuget/v/Procore.SDK.Shared.svg)](https://www.nuget.org/packages/Procore.SDK.Shared/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Procore.SDK.Shared.svg)](https://www.nuget.org/packages/Procore.SDK.Shared/)

Shared authentication, HTTP infrastructure, and common models for the Procore .NET SDK. Provides OAuth 2.0 with PKCE authentication flow, token management, and HTTP message handlers.

## Features

- 🔐 **OAuth 2.0 with PKCE**: Complete authentication flow implementation
- 🔄 **Token Management**: Automatic refresh, multiple storage options
- 📱 **Multiple Storage**: In-memory, file-based, and platform-specific secure storage
- 🔧 **HTTP Integration**: Message handlers for automatic token injection
- 🛡️ **Security**: PKCE implementation following RFC 7636 specification
- 📊 **Logging**: Comprehensive authentication event logging

## Installation

```bash
dotnet add package Procore.SDK.Shared
```

## Quick Start

```csharp
using Procore.SDK.Shared.Authentication;

// Configure authentication options
var authOptions = new ProcoreAuthOptions
{
    BaseAddress = "https://sandbox.procore.com",
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    RedirectUri = "http://localhost:8080/callback"
};

// Choose token storage
var tokenStorage = new FileTokenStorage("tokens.json");
// Or use other storage options:
// var tokenStorage = new InMemoryTokenStorage();
// var tokenStorage = new ProtectedDataTokenStorage(); // Windows DPAPI

// Create token manager
var tokenManager = new TokenManager(authOptions, tokenStorage);

// Authenticate (opens browser for OAuth flow)
await tokenManager.AuthenticateAsync();

// Use with HTTP client
var authHandler = new ProcoreAuthHandler(tokenManager);
var httpClient = new HttpClient(authHandler);
```

## Token Storage Options

### File-based Storage
```csharp
var tokenStorage = new FileTokenStorage("path/to/tokens.json");
```

### In-memory Storage
```csharp
var tokenStorage = new InMemoryTokenStorage(); // Session-only
```

### Platform Secure Storage
```csharp
var tokenStorage = new ProtectedDataTokenStorage(); // Windows DPAPI
```

## License

MIT License - see [LICENSE](https://github.com/procore/procore-sdk-dotnet/blob/main/LICENSE) file.