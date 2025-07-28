using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Procore.SDK.Core;
using Procore.SDK.ProjectManagement;
using Procore.SDK.QualitySafety;
using Procore.SDK.ConstructionFinancials;
using Procore.SDK.FieldProductivity;
using Procore.SDK.ResourceManagement;
using Procore.SDK.Shared.Authentication;
using System.Collections.Concurrent;

namespace Procore.SDK.IntegrationTests.Infrastructure;

/// <summary>
/// Test fixture for Procore sandbox environment integration testing
/// Provides authenticated clients and manages test environment setup
/// </summary>
public class ProcoredSandboxFixture : IAsyncLifetime
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProcoredSandboxFixture> _logger;
    private readonly ConcurrentDictionary<string, object> _testData = new();
    
    // Authentication
    public string ClientId { get; private set; } = string.Empty;
    public string RedirectUri { get; private set; } = string.Empty;
    public string BaseUrl { get; private set; } = string.Empty;
    public ProcoreAuthOptions AuthOptions { get; private set; } = null!;
    public ITokenManager TokenManager { get; private set; } = null!;
    public ITokenStorage TokenStorage { get; private set; } = null!;
    
    // Test Environment
    public int TestCompanyId { get; private set; }
    public string TestUserEmail { get; private set; } = string.Empty;
    public AccessToken ValidToken { get; private set; } = null!;
    
    // Clients
    public ProcoreCoreClient CoreClient { get; private set; } = null!;
    public ProjectManagementClient ProjectManagementClient { get; private set; } = null!;
    public QualitySafetyClient QualitySafetyClient { get; private set; } = null!;
    public ConstructionFinancialsClient ConstructionFinancialsClient { get; private set; } = null!;
    public FieldProductivityClient FieldProductivityClient { get; private set; } = null!;
    public ResourceManagementClient ResourceManagementClient { get; private set; } = null!;
    
    // Logging
    public ILoggerFactory LoggerFactory { get; private set; } = null!;

    public ProcoredSandboxFixture()
    {
        // Build configuration
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.integrationtest.json", optional: true)
            .AddEnvironmentVariables("PROCORE_INTEGRATION_")
            .AddUserSecrets<ProcoredSandboxFixture>()
            .Build();

        // Setup services
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
        
        LoggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = LoggerFactory.CreateLogger<ProcoredSandboxFixture>();
    }

    /// <summary>
    /// Initialize the test fixture asynchronously
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("Initializing Procore sandbox test fixture...");
            
            // Load configuration
            LoadConfiguration();
            
            // Setup authentication
            await SetupAuthenticationAsync();
            
            // Initialize clients
            InitializeClients();
            
            // Setup test environment
            await SetupTestEnvironmentAsync();
            
            _logger.LogInformation("Procore sandbox test fixture initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Procore sandbox test fixture");
            throw;
        }
    }

    /// <summary>
    /// Cleanup the test fixture
    /// </summary>
    public async Task DisposeAsync()
    {
        try
        {
            _logger.LogInformation("Disposing Procore sandbox test fixture...");
            
            // Cleanup test data
            await CleanupTestEnvironmentAsync();
            
            // Dispose clients
            CoreClient?.Dispose();
            ProjectManagementClient?.Dispose();
            QualitySafetyClient?.Dispose();
            ConstructionFinancialsClient?.Dispose();
            FieldProductivityClient?.Dispose();
            ResourceManagementClient?.Dispose();
            
            // Dispose services
            _serviceProvider?.Dispose();
            
            _logger.LogInformation("Procore sandbox test fixture disposed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing Procore sandbox test fixture");
        }
    }

    /// <summary>
    /// Gets a valid access token for testing
    /// </summary>
    public async Task<AccessToken> GetValidTokenAsync()
    {
        if (ValidToken != null && ValidToken.ExpiresAt > DateTime.UtcNow.AddMinutes(5))
        {
            return ValidToken;
        }

        // Refresh token if needed
        ValidToken = await TokenManager.GetAccessTokenAsync();
        return ValidToken;
    }

    /// <summary>
    /// Gets an authorization code for OAuth testing
    /// </summary>
    public async Task<string> GetAuthorizationCodeAsync()
    {
        // In a real test environment, this would simulate the OAuth flow
        // For testing purposes, we'll use a mock authorization code
        // or integrate with a headless browser to complete the OAuth flow
        
        await Task.Delay(100); // Simulate async operation
        return "test_authorization_code_" + Guid.NewGuid().ToString("N")[..8];
    }

    /// <summary>
    /// Creates an expired token for testing token refresh scenarios
    /// </summary>
    public AccessToken CreateExpiredToken()
    {
        return new AccessToken
        {
            Token = "expired_token_" + Guid.NewGuid().ToString("N")[..16],
            RefreshToken = "refresh_token_" + Guid.NewGuid().ToString("N")[..16],
            ExpiresAt = DateTime.UtcNow.AddMinutes(-5), // Expired 5 minutes ago
            TokenType = "Bearer"
        };
    }

    /// <summary>
    /// Creates a Core client with custom options
    /// </summary>
    public ProcoreCoreClient CreateCoreClientWithOptions(ResilienceOptions? resilienceOptions = null)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        if (resilienceOptions != null)
        {
            services.Configure<ResilienceOptions>(options =>
            {
                options.CircuitBreakerFailureThreshold = resilienceOptions.CircuitBreakerFailureThreshold;
                options.CircuitBreakerDuration = resilienceOptions.CircuitBreakerDuration;
                options.RequestTimeout = resilienceOptions.RequestTimeout;
                options.RetryAttempts = resilienceOptions.RetryAttempts;
            });
        }
        
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<ProcoreCoreClient>();
    }

    /// <summary>
    /// Creates a Core client with a specific token
    /// </summary>
    public ProcoreCoreClient CreateCoreClientWithToken(AccessToken token)
    {
        var mockTokenManager = new Mock<ITokenManager>();
        mockTokenManager.Setup(tm => tm.GetAccessTokenAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(token);

        var services = new ServiceCollection();
        ConfigureServices(services);
        services.AddSingleton(mockTokenManager.Object);
        
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<ProcoreCoreClient>();
    }

    /// <summary>
    /// Gets or creates test data of specified type
    /// </summary>
    public T GetOrCreateTestData<T>(string key, Func<T> factory)
    {
        return (T)_testData.GetOrAdd(key, _ => factory()!);
    }

    /// <summary>
    /// Creates a test project for integration testing
    /// </summary>
    public async Task<Project> GetTestProjectAsync()
    {
        const string testProjectKey = "test_project";
        
        if (_testData.TryGetValue(testProjectKey, out var existingProject))
        {
            return (Project)existingProject;
        }

        // Create a new test project
        var companies = await CoreClient.GetCompaniesAsync();
        var testCompany = companies.First(c => c.Id == TestCompanyId);

        var createRequest = new CreateProjectRequest
        {
            Name = $"Integration Test Project {DateTime.UtcNow:yyyyMMdd-HHmmss}",
            ProjectNumber = $"ITP-{Guid.NewGuid():N}"[..8],
            Description = "Created for integration testing",
            ProjectOwnerEmailAddress = TestUserEmail
        };

        var project = await ProjectManagementClient.CreateProjectAsync(testCompany.Id, createRequest);
        _testData.TryAdd(testProjectKey, project);
        
        return project;
    }

    /// <summary>
    /// Creates a logger for the specified type
    /// </summary>
    public ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

    private void ConfigureServices(IServiceCollection services)
    {
        // Configuration
        services.AddSingleton(_configuration);
        
        // Logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        // Authentication
        services.Configure<ProcoreAuthOptions>(options =>
        {
            options.ClientId = ClientId;
            options.RedirectUri = RedirectUri;
            options.BaseUrl = BaseUrl;
            options.Scopes = new[] { "read_company_directory", "read_project_directory" };
        });

        services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
        services.AddTransient<ITokenManager, TokenManager>();
        services.AddTransient<OAuthFlowHelper>();

        // HTTP clients
        services.AddHttpClient();

        // SDK clients
        services.AddProcoreSDK(options =>
        {
            options.BaseUrl = BaseUrl;
            options.ClientId = ClientId;
        });
    }

    private void LoadConfiguration()
    {
        ClientId = _configuration["Procore:ClientId"] 
                  ?? throw new InvalidOperationException("Procore:ClientId not configured");
        
        RedirectUri = _configuration["Procore:RedirectUri"] 
                     ?? "https://localhost:5001/auth/callback";
        
        BaseUrl = _configuration["Procore:BaseUrl"] 
                 ?? "https://sandbox.procore.com";
        
        TestCompanyId = int.Parse(_configuration["Procore:TestCompanyId"] ?? "0");
        
        TestUserEmail = _configuration["Procore:TestUserEmail"] 
                       ?? "test@example.com";

        AuthOptions = new ProcoreAuthOptions
        {
            ClientId = ClientId,
            RedirectUri = RedirectUri,
            BaseUrl = BaseUrl,
            Scopes = new[] { "read_company_directory", "read_project_directory" }
        };
    }

    private async Task SetupAuthenticationAsync()
    {
        TokenStorage = new InMemoryTokenStorage();
        TokenManager = new TokenManager(AuthOptions, TokenStorage);
        
        // In a real test environment, this would complete the OAuth flow
        // For now, we'll create a mock valid token
        ValidToken = new AccessToken
        {
            Token = "sandbox_access_token_" + Guid.NewGuid().ToString("N"),
            RefreshToken = "sandbox_refresh_token_" + Guid.NewGuid().ToString("N"),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            TokenType = "Bearer"
        };

        await TokenStorage.StoreTokenAsync(ValidToken);
    }

    private void InitializeClients()
    {
        CoreClient = _serviceProvider.GetRequiredService<ProcoreCoreClient>();
        ProjectManagementClient = _serviceProvider.GetRequiredService<ProjectManagementClient>();
        QualitySafetyClient = _serviceProvider.GetRequiredService<QualitySafetyClient>();
        ConstructionFinancialsClient = _serviceProvider.GetRequiredService<ConstructionFinancialsClient>();
        FieldProductivityClient = _serviceProvider.GetRequiredService<FieldProductivityClient>();
        ResourceManagementClient = _serviceProvider.GetRequiredService<ResourceManagementClient>();
    }

    private async Task SetupTestEnvironmentAsync()
    {
        _logger.LogInformation("Setting up test environment...");
        
        try
        {
            // Validate connection to sandbox
            var companies = await CoreClient.GetCompaniesAsync();
            _logger.LogInformation($"Connected to sandbox, found {companies.Count()} companies");
            
            // Ensure test company exists
            if (TestCompanyId > 0)
            {
                var testCompany = companies.FirstOrDefault(c => c.Id == TestCompanyId);
                if (testCompany == null)
                {
                    _logger.LogWarning($"Test company {TestCompanyId} not found, using first available company");
                    TestCompanyId = companies.First().Id;
                }
            }
            else
            {
                TestCompanyId = companies.First().Id;
            }
            
            _logger.LogInformation($"Using test company ID: {TestCompanyId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to setup test environment");
            throw;
        }
    }

    private async Task CleanupTestEnvironmentAsync()
    {
        _logger.LogInformation("Cleaning up test environment...");
        
        try
        {
            // Cleanup any test data created during tests
            foreach (var kvp in _testData)
            {
                if (kvp.Value is Project project)
                {
                    try
                    {
                        await ProjectManagementClient.DeleteProjectAsync(TestCompanyId, project.Id);
                        _logger.LogInformation($"Cleaned up test project: {project.Id}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Failed to cleanup test project {project.Id}");
                    }
                }
            }
            
            _testData.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during test environment cleanup");
        }
    }
}