# Procore SDK for .NET

A modern, comprehensive .NET SDK for the Procore construction management platform. This SDK provides a clean, strongly-typed interface for accessing Procore's REST API with full OAuth 2.0 authentication support.

## Features

- **🔐 Complete OAuth 2.0 with PKCE Support** - Secure authentication following industry standards
- **📦 Modular Architecture** - Separate NuGet packages for different resource groups
- **🔄 Automatic Token Management** - Transparent token refresh and caching
- **🏗️ Generated Client Libraries** - Type-safe API clients generated from OpenAPI specifications
- **💉 Dependency Injection Ready** - Full support for .NET DI container
- **🧪 Thoroughly Tested** - Comprehensive test suite with 82%+ coverage
- **📝 Rich Documentation** - Complete XML documentation and examples

## Quick Start

### Installation

```bash
# Install the main SDK package
dotnet add package Procore.SDK

# Or install specific resource packages
dotnet add package Procore.SDK.Core
dotnet add package Procore.SDK.ProjectManagement
```

### Basic Usage

```csharp
using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Extensions;

// Configure services
var services = new ServiceCollection();
services.AddProcoreSDK(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.BaseAddress = "https://api.procore.com";
});

var serviceProvider = services.BuildServiceProvider();

// Use the SDK
var coreClient = serviceProvider.GetRequiredService<ICoreClient>();
var companies = await coreClient.GetCompaniesAsync();
```

## Package Structure

| Package | Description | Resources |
|---------|-------------|-----------|
| **Procore.SDK** | Main aggregator package | All clients + DI setup |
| **Procore.SDK.Shared** | Authentication & common utilities | OAuth, HTTP handlers |
| **Procore.SDK.Core** | Core business entities | Companies, Users, Documents |
| **Procore.SDK.ProjectManagement** | Project workflows | Projects, Tasks, Schedules |
| **Procore.SDK.QualitySafety** | Safety & compliance | Inspections, Incidents |
| **Procore.SDK.ConstructionFinancials** | Financial operations | Contracts, Budgets, Costs |
| **Procore.SDK.FieldProductivity** | Field operations | Daily logs, Timecards |
| **Procore.SDK.ResourceManagement** | Resource allocation | Personnel, Equipment |

## Authentication

The SDK supports OAuth 2.0 Authorization Code flow with PKCE for maximum security:

```csharp
// Configure authentication
services.AddProcoreSDK(options =>
{
    options.ClientId = "your-client-id";
    options.ClientSecret = "your-client-secret";
    options.UsePkce = true; // Recommended
    options.TokenRefreshMargin = TimeSpan.FromMinutes(5);
});
```

### Token Storage Options

- **In-Memory**: For development and testing
- **File-based**: Encrypted local storage
- **Protected Data**: Windows DPAPI integration

## Documentation

- [Getting Started Guide](docs/getting-started.md)
- [Authentication Setup](docs/authentication.md)
- [API Reference](docs/api-reference.md)
- [Examples](samples/)

## Development

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code
- Procore API credentials

### Building

```bash
git clone https://github.com/bskertchly/procure-sdk.git
cd procore-sdk
dotnet restore
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Generating Clients

```bash
# PowerShell
.\tools\Generate-Clients.ps1

# Bash
./tools/generate-clients.sh
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## Architecture

The SDK follows a modular architecture with clean separation of concerns:

```
Procore.SDK/
├── src/
│   ├── Procore.SDK.Shared/          # Authentication & common utilities
│   ├── Procore.SDK/                 # Main SDK & DI setup
│   ├── Procore.SDK.Core/            # Core business entities
│   └── Procore.SDK.*/               # Resource-specific clients
├── tests/
│   └── Procore.SDK.*.Tests/         # Comprehensive test suite
├── samples/
│   ├── ConsoleSample/               # Console application example
│   └── WebSample/                   # ASP.NET Core example
└── tools/
    └── Generate-*.ps1               # Client generation scripts
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- 📖 [Documentation](docs/)
- 🐛 [Issue Tracker](https://github.com/bskertchly/procure-sdk/issues)
- 💬 [Discussions](https://github.com/bskertchly/procure-sdk/discussions)

---

Built with ❤️ for the construction industry.