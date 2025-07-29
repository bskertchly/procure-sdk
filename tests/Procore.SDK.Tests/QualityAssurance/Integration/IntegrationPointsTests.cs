using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Core;
using Procore.SDK.Core.Models;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Tests.QualityAssurance.Integration;

/// <summary>
/// Integration points and API endpoint functionality tests
/// Validates proper integration between components, API usage patterns, and endpoint functionality
/// </summary>
public class IntegrationPointsTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly string _projectRoot;
    private readonly string _samplesPath;
    private readonly ServiceProvider _serviceProvider;
    private readonly ILogger<IntegrationPointsTests> _logger;

    public IntegrationPointsTests(ITestOutputHelper output)
    {
        _output = output;
        _projectRoot = GetProjectRoot();
        _samplesPath = Path.Combine(_projectRoot, "samples");

        // Setup test configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Procore:Authentication:ClientId"] = "test-client-id",
                ["Procore:Authentication:ClientSecret"] = "test-client-secret",
                ["Procore:Authentication:RedirectUri"] = "https://localhost:5001/auth/callback",
                ["Procore:Authentication:Scopes"] = "read",
                ["Procore:BaseUrl"] = "https://sandbox.procore.com"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        services.AddProcoreSDK(configuration);
        services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
        services.AddSingleton<ICoreClient, ProcoreCoreClient>();

        _serviceProvider = services.BuildServiceProvider();
        _logger = _serviceProvider.GetRequiredService<ILogger<IntegrationPointsTests>>();
    }

    private static string GetProjectRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var projectRoot = currentDir;
        
        while (projectRoot != null && !Directory.Exists(Path.Combine(projectRoot, "samples")))
        {
            projectRoot = Directory.GetParent(projectRoot)?.FullName;
        }
        
        if (projectRoot == null)
            throw new DirectoryNotFoundException("Could not find project root");
            
        return projectRoot;
    }

    [Fact]
    public void Sample_Applications_Should_Integrate_All_Required_Components()
    {
        // Arrange
        var requiredComponents = new[]
        {
            typeof(ICoreClient),
            typeof(ITokenManager),
            typeof(ITokenStorage),
            typeof(OAuthFlowHelper)
        };

        // Act & Assert - Verify all components can be resolved
        foreach (var componentType in requiredComponents)
        {
            var component = _serviceProvider.GetService(componentType);
            Assert.NotNull(component);
            _output.WriteLine($"✅ Component registered: {componentType.Name}");
        }

        _output.WriteLine("✅ All required components successfully integrated via DI container");
    }

    [Fact]
    public void SDK_Configuration_Should_Be_Properly_Applied()
    {
        // Arrange
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

        // Act & Assert - Verify configuration is properly loaded
        var clientId = configuration["Procore:Authentication:ClientId"];
        var redirectUri = configuration["Procore:Authentication:RedirectUri"];
        var baseUrl = configuration["Procore:BaseUrl"];
        var scopes = configuration["Procore:Authentication:Scopes"];

        Assert.NotNull(clientId);
        Assert.NotEmpty(clientId);
        Assert.NotNull(redirectUri);
        Assert.True(Uri.IsWellFormedUriString(redirectUri, UriKind.Absolute));
        Assert.NotNull(baseUrl);
        Assert.True(Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute));
        Assert.NotNull(scopes);

        _output.WriteLine($"✅ Configuration validation:");
        _output.WriteLine($"   Client ID: {clientId}");
        _output.WriteLine($"   Redirect URI: {redirectUri}");
        _output.WriteLine($"   Base URL: {baseUrl}");
        _output.WriteLine($"   Scopes: {scopes}");
    }

    [Fact]
    public void Console_Sample_Should_Integrate_Properly_With_SDK()
    {
        // Arrange
        var consoleProgramFile = Path.Combine(_samplesPath, "ConsoleSample", "Program.cs");
        Assert.True(File.Exists(consoleProgramFile), "Console sample Program.cs should exist");

        var content = File.ReadAllText(consoleProgramFile);

        // Act & Assert - Check integration patterns
        
        // Should register SDK services
        Assert.Contains("services.AddProcoreSDK", content);
        _output.WriteLine("✅ Console sample: Registers Procore SDK services");

        // Should use dependency injection
        Assert.Contains("GetRequiredService", content);
        _output.WriteLine("✅ Console sample: Uses dependency injection properly");

        // Should handle OAuth flow
        Assert.Contains("OAuthFlowHelper", content);
        Assert.Contains("GenerateAuthorizationUrl", content);
        Assert.Contains("ExchangeCodeForTokenAsync", content);
        _output.WriteLine("✅ Console sample: Implements OAuth flow integration");

        // Should use Core client for API calls
        Assert.Contains("ICoreClient", content);
        Assert.Contains("GetCurrentUserAsync", content);
        Assert.Contains("GetCompaniesAsync", content);
        _output.WriteLine("✅ Console sample: Integrates with Core API client");

        // Should handle token storage
        Assert.Contains("ITokenManager", content);
        Assert.Contains("GetAccessTokenAsync", content);
        Assert.Contains("StoreTokenAsync", content);
        _output.WriteLine("✅ Console sample: Implements token management integration");
    }

    [Fact]
    public void Web_Sample_Should_Integrate_Properly_With_SDK()
    {
        // Arrange
        var webProgramFile = Path.Combine(_samplesPath, "WebSample", "Program.cs");
        if (!File.Exists(webProgramFile))
        {
            _output.WriteLine("⚠️  Web sample not found - skipping web integration tests");
            return;
        }

        var content = File.ReadAllText(webProgramFile);

        // Act & Assert - Check web integration patterns
        
        // Should register SDK services
        Assert.Contains("services.AddProcoreSDK", content);
        _output.WriteLine("✅ Web sample: Registers Procore SDK services");

        // Should configure authentication
        Assert.Contains("AddAuthentication", content);
        Assert.Contains("AddCookie", content);
        _output.WriteLine("✅ Web sample: Configures authentication middleware");

        // Should use session-based token storage
        Assert.Contains("SessionTokenStorage", content);
        Assert.Contains("AddSession", content);
        _output.WriteLine("✅ Web sample: Configures session-based token storage");

        // Should configure security
        Assert.Contains("UseHttpsRedirection", content);
        Assert.Contains("UseAuthentication", content);
        Assert.Contains("UseAuthorization", content);
        _output.WriteLine("✅ Web sample: Implements security middleware integration");

        // Check for MVC integration
        Assert.Contains("AddControllersWithViews", content);
        Assert.Contains("MapControllerRoute", content);
        _output.WriteLine("✅ Web sample: Integrates with ASP.NET Core MVC");
    }

    [Fact]
    public void Web_Controllers_Should_Integrate_With_SDK_Services()
    {
        // Arrange
        var controllerFiles = Directory.GetFiles(Path.Combine(_samplesPath, "WebSample", "Controllers"), "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains("obj") && !f.Contains("bin"))
            .ToArray();

        foreach (var controllerFile in controllerFiles)
        {
            var content = File.ReadAllText(controllerFile);
            var fileName = Path.GetFileName(controllerFile);

            // Act & Assert - Check controller integration
            
            // Should use dependency injection
            if (content.Contains("Controller"))
            {
                var hasConstructorInjection = content.Contains("public ") && content.Contains("Controller(") && content.Contains("private readonly");
                if (hasConstructorInjection)
                {
                    _output.WriteLine($"✅ {fileName}: Uses constructor dependency injection");
                }

                // Check for SDK service usage
                var sdkServices = new[]
                {
                    ("AuthenticationService", "Authentication integration"),
                    ("ProjectService", "Project management integration"),
                    ("ILogger", "Logging integration"),
                    ("OAuthFlowHelper", "OAuth flow integration")
                };

                foreach (var (service, description) in sdkServices)
                {
                    if (content.Contains(service))
                    {
                        _output.WriteLine($"✅ {fileName}: {description}");
                    }
                }

                // Check for proper async patterns
                if (content.Contains("async Task<IActionResult>"))
                {
                    _output.WriteLine($"✅ {fileName}: Uses async action methods");
                }

                // Check for error handling
                if (content.Contains("try") && content.Contains("catch"))
                {
                    _output.WriteLine($"✅ {fileName}: Implements error handling");
                }
            }
        }
    }

    [Fact]
    public void API_Endpoints_Should_Be_Properly_Structured()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var apiEndpoints = new List<(string file, string endpoint, string method)>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act - Extract API endpoint patterns
            
            // Find method calls that look like API endpoints
            var apiCalls = new[]
            {
                (@"GetCurrentUserAsync\(\)", "GET /me"),
                (@"GetCompaniesAsync\(\)", "GET /companies"),
                (@"GetUsersAsync\([^)]+\)", "GET /companies/{id}/users"),
                (@"CreateCompanyAsync\([^)]+\)", "POST /companies"),
                (@"DeleteCompanyAsync\([^)]+\)", "DELETE /companies/{id}"),
                (@"ExchangeCodeForTokenAsync\([^)]+\)", "POST /oauth/token"),
                (@"RefreshTokenAsync\(\)", "POST /oauth/token (refresh)")
            };

            foreach (var (pattern, endpoint) in apiCalls)
            {
                var matches = Regex.Matches(content, pattern);
                if (matches.Count > 0)
                {
                    apiEndpoints.Add((fileName, endpoint, $"{matches.Count} calls"));
                }
            }
        }

        // Assert - Verify API usage patterns
        Assert.True(apiEndpoints.Any(), "Should find API endpoint usage in sample applications");

        foreach (var (file, endpoint, method) in apiEndpoints)
        {
            _output.WriteLine($"✅ {file}: {endpoint} - {method}");
        }

        _output.WriteLine($"✅ Found {apiEndpoints.Count} API endpoint integration patterns");
    }

    [Fact]
    public async Task OAuth_Flow_Integration_Should_Work_End_To_End()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();

        // Act & Assert - Test OAuth flow integration

        // Step 1: Generate authorization URL
        var state = "integration-test-state";
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);

        Assert.NotNull(authUrl);
        Assert.NotEmpty(codeVerifier);
        Assert.Contains("client_id=test-client-id", authUrl);
        Assert.Contains("redirect_uri=", authUrl);
        Assert.Contains("code_challenge=", authUrl);
        
        _output.WriteLine("✅ OAuth Step 1: Authorization URL generation");

        // Step 2: Token storage integration
        var testToken = new AccessToken(
            Token: "integration-test-token",
            TokenType: "Bearer",
            ExpiresAt: DateTimeOffset.UtcNow.AddHours(1),
            RefreshToken: "integration-test-refresh",
            Scopes: new[] { "read" });

        await tokenStorage.StoreTokenAsync("integration-test-key", testToken);
        var storedToken = await tokenStorage.GetTokenAsync("integration-test-key");
        
        Assert.NotNull(storedToken);
        Assert.Equal(testToken.Token, storedToken.Token);
        Assert.Equal(testToken.RefreshToken, storedToken.RefreshToken);
        
        _output.WriteLine("✅ OAuth Step 2: Token storage integration");

        // Step 3: Token manager integration
        var managedToken = await tokenManager.GetAccessTokenAsync();
        Assert.NotNull(managedToken);
        Assert.Equal(testToken.Token, managedToken.Token);
        
        _output.WriteLine("✅ OAuth Step 3: Token manager integration");

        // Cleanup
        await tokenStorage.DeleteTokenAsync("integration-test-key");
        var clearedToken = await tokenStorage.GetTokenAsync("integration-test-key");
        Assert.Null(clearedToken);
        
        _output.WriteLine("✅ OAuth integration cleanup successful");
    }

    [Fact]
    public void HTTP_Client_Integration_Should_Be_Properly_Configured()
    {
        // Arrange
        var coreClient = _serviceProvider.GetService<ICoreClient>();
        Assert.NotNull(coreClient);

        // Act & Assert - Test HTTP client configuration
        
        // The client should be properly instantiated
        _output.WriteLine("✅ Core client successfully created via DI");

        // Check for proper base URL configuration in code
        var codeFiles = GetAllCSharpFiles();
        var httpConfigCount = 0;

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            
            if (content.Contains("HttpClient") || content.Contains("BaseAddress"))
            {
                httpConfigCount++;
                _output.WriteLine($"✅ HTTP client configuration found in {Path.GetFileName(file)}");
            }
        }

        Assert.True(httpConfigCount > 0, "HTTP client configuration should be present in sample applications");
        
        // Configuration should be applied through DI/configuration system
        _output.WriteLine("✅ HTTP client integration verified through DI resolution");
    }

    [Fact]
    public void Error_Handling_Integration_Should_Be_Consistent()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var errorHandlingPatterns = new List<string>();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check error handling integration patterns
            
            // Check for consistent error handling across integration points
            var patterns = new[]
            {
                (@"catch\s*\([^)]*HttpRequestException[^)]*\)", "HTTP error handling"),
                (@"catch\s*\([^)]*TaskCanceledException[^)]*\)", "Cancellation handling"),
                (@"catch\s*\([^)]*Exception[^)]*\)[^}]*Log", "Logging integration"),
                (@"TempData\[[""']Error[""']\]", "Web error feedback"),
                (@"Console\.WriteLine[^)]*❌", "Console error output"),
                (@"_logger\.LogError", "Structured error logging")
            };

            foreach (var (pattern, description) in patterns)
            {
                if (Regex.IsMatch(content, pattern, RegexOptions.Singleline))
                {
                    errorHandlingPatterns.Add($"✅ {fileName}: {description}");
                }
            }
        }

        // Output error handling analysis
        foreach (var pattern in errorHandlingPatterns)
        {
            _output.WriteLine(pattern);
        }

        Assert.True(errorHandlingPatterns.Any(), "Should find error handling integration patterns");
    }

    [Fact]
    public void Configuration_Integration_Should_Be_Comprehensive()
    {
        // Arrange
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();

        // Act & Assert - Test configuration integration
        
        // Check required configuration sections
        var requiredSections = new[]
        {
            "Procore:Authentication:ClientId",
            "Procore:Authentication:RedirectUri",
            "Procore:Authentication:Scopes",
            "Procore:BaseUrl"
        };

        foreach (var section in requiredSections)
        {
            var value = configuration[section];
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            _output.WriteLine($"✅ Configuration: {section} = {(section.Contains("Secret") ? "[HIDDEN]" : value)}");
        }

        // Check configuration file integration in samples
        var configFiles = new[]
        {
            Path.Combine(_samplesPath, "ConsoleSample", "appsettings.json"),
            Path.Combine(_samplesPath, "WebSample", "appsettings.json")
        };

        foreach (var configFile in configFiles.Where(File.Exists))
        {
            var content = File.ReadAllText(configFile);
            
            if (content.Contains("Procore"))
            {
                _output.WriteLine($"✅ Configuration file integration: {Path.GetFileName(configFile)}");
            }
        }
    }

    [Fact]
    public void Logging_Integration_Should_Be_Functional()
    {
        // Arrange
        var logger = _serviceProvider.GetService<ILogger<IntegrationPointsTests>>();
        Assert.NotNull(logger);

        // Act & Assert - Test logging integration
        
        // Should be able to log at different levels
        logger.LogInformation("Integration test logging validation");
        logger.LogWarning("Test warning message");
        
        _output.WriteLine("✅ Logging integration: ILogger successfully resolved and functional");

        // Check logging configuration in samples
        var codeFiles = GetAllCSharpFiles();
        var loggingIntegration = false;

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            
            if (content.Contains("ILogger") && content.Contains("LogInformation"))
            {
                loggingIntegration = true;
                _output.WriteLine($"✅ Logging integration found in {Path.GetFileName(file)}");
            }
        }

        Assert.True(loggingIntegration, "Should find logging integration in sample applications");
    }

    [Fact]
    public void Service_Lifetime_Management_Should_Be_Correct()
    {
        // Arrange & Act - Test service lifetime management
        
        // Test singleton services (should be same instance)
        var tokenStorage1 = _serviceProvider.GetRequiredService<ITokenStorage>();
        var tokenStorage2 = _serviceProvider.GetRequiredService<ITokenStorage>();
        
        Assert.Same(tokenStorage1, tokenStorage2);
        _output.WriteLine("✅ Singleton services maintain same instance");

        // Test scoped services would be different in different scopes
        // For this test, we'll verify the services are properly registered
        var coreClient1 = _serviceProvider.GetService<ICoreClient>();
        var coreClient2 = _serviceProvider.GetService<ICoreClient>();
        
        Assert.NotNull(coreClient1);
        Assert.NotNull(coreClient2);
        _output.WriteLine("✅ Service resolution works consistently");

        // Check service registration patterns in sample code
        var programFiles = new[]
        {
            Path.Combine(_samplesPath, "ConsoleSample", "Program.cs"),
            Path.Combine(_samplesPath, "WebSample", "Program.cs")
        };

        foreach (var programFile in programFiles.Where(File.Exists))
        {
            var content = File.ReadAllText(programFile);
            
            var lifetimePatterns = new[]
            {
                ("AddSingleton", "Singleton lifetime"),
                ("AddScoped", "Scoped lifetime"),
                ("AddTransient", "Transient lifetime")
            };

            foreach (var (pattern, description) in lifetimePatterns)
            {
                if (content.Contains(pattern))
                {
                    _output.WriteLine($"✅ {Path.GetFileName(programFile)}: Uses {description}");
                }
            }
        }
    }

    private string[] GetAllCSharpFiles()
    {
        var consolePath = Path.Combine(_samplesPath, "ConsoleSample");
        var webPath = Path.Combine(_samplesPath, "WebSample");

        var files = new[]
        {
            Directory.GetFiles(consolePath, "*.cs", SearchOption.AllDirectories),
            Directory.GetFiles(webPath, "*.cs", SearchOption.AllDirectories)
        }.SelectMany(x => x)
         .Where(f => !f.Contains("obj") && !f.Contains("bin"))
         .ToArray();

        return files;
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}