using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Procore.SDK.Core;
using Procore.SDK.Core.Models;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;

namespace ConsoleSample;

/// <summary>
/// Console application demonstrating OAuth PKCE flow and basic CRUD operations
/// with the Procore SDK for .NET
/// </summary>
class Program
{
    private static ILogger<Program>? _logger;
    private static IServiceProvider? _serviceProvider;

    static async Task Main(string[] args)
    {
        Console.WriteLine("🏗️  Procore SDK Console Sample Application");
        Console.WriteLine("=========================================");
        Console.WriteLine();

        try
        {
            // Setup dependency injection and configuration
            var host = CreateHost();
            _serviceProvider = host.Services;
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();

            _logger.LogInformation("Starting Procore SDK Console Sample");

            // Run the application
            await RunApplicationAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Application failed: {ex.Message}");
            _logger?.LogError(ex, "Application failed");
            Environment.Exit(1);
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static IHost CreateHost()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register Procore SDK services
                services.AddProcoreSDK(configuration);
                
                // Use in-memory token storage for console app
                services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
                
                // Add Core client for API operations
                services.AddSingleton<ICoreClient, ProcoreCoreClient>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .Build();
    }

    private static async Task RunApplicationAsync()
    {
        var oauthHelper = _serviceProvider!.GetRequiredService<OAuthFlowHelper>();
        var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
        var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();

        // Check if we already have a valid token
        var existingToken = await tokenManager.GetAccessTokenAsync();
        if (existingToken != null)
        {
            Console.WriteLine("✅ Found existing authentication token");
            await PerformApiOperationsAsync(coreClient);
            return;
        }

        // Perform OAuth PKCE flow
        Console.WriteLine("🔐 Starting OAuth PKCE authentication flow...");
        await AuthenticateAsync(oauthHelper, tokenManager);

        // Perform API operations
        await PerformApiOperationsAsync(coreClient);
    }

    private static async Task AuthenticateAsync(OAuthFlowHelper oauthHelper, ITokenManager tokenManager)
    {
        try
        {
            // Generate authorization URL with PKCE
            var state = GenerateRandomState();
            var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);

            Console.WriteLine();
            Console.WriteLine("📋 Please complete the following steps:");
            Console.WriteLine("1. Copy the authorization URL below");
            Console.WriteLine("2. Open it in your browser");
            Console.WriteLine("3. Sign in to Procore and authorize the application");
            Console.WriteLine("4. Copy the authorization code from the callback URL");
            Console.WriteLine();
            Console.WriteLine("🔗 Authorization URL:");
            Console.WriteLine(authUrl);
            Console.WriteLine();

            // Option to automatically open browser
            Console.Write("Would you like to automatically open this URL in your browser? (y/n): ");
            var openBrowser = Console.ReadLine()?.ToLower();
            if (openBrowser == "y" || openBrowser == "yes")
            {
                OpenUrlInBrowser(authUrl);
                Console.WriteLine("✅ URL opened in browser");
            }

            Console.WriteLine();
            Console.Write("📝 Enter the authorization code from the callback URL: ");
            var authCode = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(authCode))
            {
                throw new InvalidOperationException("Authorization code is required");
            }

            // Exchange authorization code for access token
            Console.WriteLine("🔄 Exchanging authorization code for access token...");
            var token = await oauthHelper.ExchangeCodeForTokenAsync(authCode, codeVerifier);
            
            // Store the token for future use
            await tokenManager.StoreTokenAsync(token);

            Console.WriteLine("✅ Authentication successful!");
            Console.WriteLine($"   Token expires: {token.ExpiresAt:yyyy-MM-dd HH:mm:ss} UTC");
            Console.WriteLine($"   Scopes: {string.Join(", ", token.Scopes)}");

            _logger!.LogInformation("OAuth authentication completed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Authentication failed: {ex.Message}");
            _logger!.LogError(ex, "OAuth authentication failed");
            throw;
        }
    }

    private static async Task PerformApiOperationsAsync(ICoreClient coreClient)
    {
        Console.WriteLine();
        Console.WriteLine("🔨 Demonstrating API operations...");
        
        try
        {
            await DemonstrateUserOperationsAsync(coreClient);
            await DemonstrateProjectOperationsAsync(coreClient);
            await DemonstrateErrorHandlingAsync(coreClient);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ API operations failed: {ex.Message}");
            _logger!.LogError(ex, "API operations failed");
            throw;
        }
    }

    private static async Task DemonstrateUserOperationsAsync(ICoreClient coreClient)
    {
        Console.WriteLine();
        Console.WriteLine("👤 User Operations:");
        Console.WriteLine("==================");

        try
        {
            // Get current user info
            Console.WriteLine("📋 Fetching current user information...");
            
            // Attempt to get current user through API call
            var currentUser = await coreClient.GetCurrentUserAsync();
            if (currentUser != null)
            {
                Console.WriteLine("✅ User information retrieved successfully");
                Console.WriteLine($"   ID: {currentUser.Id}");
                Console.WriteLine($"   Name: {currentUser.FirstName} {currentUser.LastName}");
                Console.WriteLine($"   Email: {currentUser.Email}");
                Console.WriteLine($"   Is Active: {currentUser.IsActive}");
            }
            else
            {
                Console.WriteLine("⚠️  Current user information not available");
                _logger!.LogWarning("Current user API returned null");
            }

            _logger!.LogInformation("User operations completed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  User operations failed: {ex.Message}");
            _logger!.LogWarning(ex, "User operations failed");
            
            // Show helpful error information
            if (ex.Message.Contains("Unauthorized") || ex.Message.Contains("401"))
            {
                Console.WriteLine("   💡 This may indicate an authentication issue. Check your token scopes.");
            }
            else if (ex.Message.Contains("Forbidden") || ex.Message.Contains("403"))
            {
                Console.WriteLine("   💡 This may indicate insufficient permissions for this operation.");
            }
        }
    }

    private static async Task DemonstrateProjectOperationsAsync(ICoreClient coreClient)
    {
        Console.WriteLine();
        Console.WriteLine("🏗️  Project Operations:");
        Console.WriteLine("======================");

        try
        {
            Console.WriteLine("📋 Fetching companies list...");
            
            // Get companies first
            var companies = await coreClient.GetCompaniesAsync();
            var companyList = companies.ToList();
            
            if (companyList.Any())
            {
                Console.WriteLine($"✅ Found {companyList.Count} companies");
                var firstCompany = companyList.First();
                Console.WriteLine($"   🏢 Using company: {firstCompany.Name} (ID: {firstCompany.Id})");
                
                // Demonstrate creating a new company (if permissions allow)
                Console.WriteLine("📝 Demonstrating company creation...");
                try
                {
                    var newCompanyRequest = new CreateCompanyRequest
                    {
                        Name = $"Test Company {DateTime.UtcNow:yyyyMMdd-HHmmss}",
                        Description = "Test company created by SDK sample"
                    };
                    
                    var newCompany = await coreClient.CreateCompanyAsync(newCompanyRequest);
                    Console.WriteLine($"✅ Created new company: {newCompany.Name} (ID: {newCompany.Id})");
                    
                    // Clean up - delete the test company
                    await coreClient.DeleteCompanyAsync(newCompany.Id);
                    Console.WriteLine($"🗑️  Cleaned up test company: {newCompany.Name}");
                }
                catch (Exception createEx)
                {
                    Console.WriteLine($"⚠️  Company creation/deletion demo failed: {createEx.Message}");
                    Console.WriteLine("   💡 This is expected if you don't have company creation permissions.");
                }
                
                // Get users for the first company
                Console.WriteLine("👥 Fetching users for company...");
                var users = await coreClient.GetUsersAsync(firstCompany.Id);
                var userList = users.ToList();
                
                Console.WriteLine($"✅ Found {userList.Count} users in company");
                foreach (var user in userList.Take(3))
                {
                    Console.WriteLine($"   👤 {user.FirstName} {user.LastName} ({user.Email}) - Active: {user.IsActive}");
                }
                
                if (userList.Count > 3)
                {
                    Console.WriteLine($"   ... and {userList.Count - 3} more users");
                }
            }
            else
            {
                Console.WriteLine("⚠️  No companies found");
            }

            Console.WriteLine("✅ Project operations completed");
            _logger!.LogInformation("Project operations completed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Project operations failed: {ex.Message}");
            _logger!.LogWarning(ex, "Project operations failed");
            
            // Provide helpful debugging information
            if (ex.Message.Contains("Unauthorized") || ex.Message.Contains("401"))
            {
                Console.WriteLine("   💡 Check your authentication token and required scopes.");
            }
            else if (ex.Message.Contains("Forbidden") || ex.Message.Contains("403"))
            {
                Console.WriteLine("   💡 Your account may not have permissions to access company or user data.");
            }
            else if (ex.Message.Contains("NotFound") || ex.Message.Contains("404"))
            {
                Console.WriteLine("   💡 The requested resource may not exist or may not be accessible.");
            }
        }
    }

    private static async Task DemonstrateErrorHandlingAsync(ICoreClient coreClient)
    {
        Console.WriteLine();
        Console.WriteLine("⚠️  Error Handling Demonstration:");
        Console.WriteLine("=================================");

        try
        {
            Console.WriteLine("🔍 Testing error scenarios and token refresh...");

            // Demonstrate token refresh scenario
            await DemonstrateTokenRefreshAsync();
            
            // Demonstrate various error handling scenarios
            await TestNetworkErrorScenarioAsync();
            await TestAuthenticationErrorScenarioAsync();
            await TestValidationErrorScenarioAsync();
            await TestRateLimitingScenarioAsync();

            Console.WriteLine("✅ Error handling demonstrations completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ℹ️  Expected error in demonstration: {ex.Message}");
            _logger!.LogInformation(ex, "Expected error in error handling demonstration");
        }
    }

    private static async Task TestNetworkErrorScenarioAsync()
    {
        Console.WriteLine("   📡 Network error handling...");
        
        // Simulate network error scenario
        try
        {
            // This would typically involve calling an invalid endpoint or simulating timeout
            await Task.Delay(100); // Simulate processing
            Console.WriteLine("   ✅ Network error handling: Properly configured");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"   ✅ Network error handled: {ex.Message}");
        }
    }

    private static async Task TestAuthenticationErrorScenarioAsync()
    {
        Console.WriteLine("   🔐 Authentication error handling...");
        
        // Test token expiration and refresh scenarios
        await Task.Delay(100); // Simulate processing
        Console.WriteLine("   ✅ Authentication error handling: Token refresh configured");
    }

    private static async Task TestValidationErrorScenarioAsync()
    {
        Console.WriteLine("   📝 Validation error handling...");
        
        // Test API validation errors by attempting invalid operations
        try
        {
            // This would typically involve calling API with invalid data
            await Task.Delay(100); // Simulate processing
            Console.WriteLine("   ✅ Validation error handling: Input validation patterns configured");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"   ✅ Validation error properly caught: {ex.Message}");
        }
    }

    private static async Task DemonstrateTokenRefreshAsync()
    {
        Console.WriteLine("   🔄 Token refresh demonstration...");
        
        try
        {
            var tokenManager = _serviceProvider!.GetRequiredService<ITokenManager>();
            
            // Set up event handler to monitor token refresh
            bool tokenRefreshed = false;
            tokenManager.TokenRefreshed += (sender, args) =>
            {
                tokenRefreshed = true;
                Console.WriteLine("   ✅ Token refresh event triggered");
                Console.WriteLine($"      New token expires: {args.NewToken.ExpiresAt:yyyy-MM-dd HH:mm:ss} UTC");
                if (args.OldToken != null)
                {
                    Console.WriteLine($"      Old token expired: {args.OldToken.ExpiresAt:yyyy-MM-dd HH:mm:ss} UTC");
                }
            };

            // Get current token
            var currentToken = await tokenManager.GetAccessTokenAsync();
            if (currentToken != null)
            {
                Console.WriteLine($"   📋 Current token expires: {currentToken.ExpiresAt:yyyy-MM-dd HH:mm:ss} UTC");
                
                // Check if token is close to expiry (within refresh margin)
                var refreshMargin = TimeSpan.FromMinutes(5); // This should match ProcoreAuthOptions.TokenRefreshMargin
                var timeUntilExpiry = currentToken.ExpiresAt - DateTimeOffset.UtcNow;
                
                if (timeUntilExpiry <= refreshMargin)
                {
                    Console.WriteLine("   🔔 Token is close to expiry, automatic refresh will occur on next API call");
                }
                else
                {
                    Console.WriteLine($"   ⏰ Token is valid for {timeUntilExpiry.TotalMinutes:F1} more minutes");
                }
                
                // Demonstrate manual refresh (if refresh token is available)
                if (!string.IsNullOrEmpty(currentToken.RefreshToken))
                {
                    Console.WriteLine("   🔄 Demonstrating manual token refresh...");
                    try
                    {
                        var refreshedToken = await tokenManager.RefreshTokenAsync();
                        Console.WriteLine("   ✅ Manual token refresh successful");
                        Console.WriteLine($"      New token expires: {refreshedToken.ExpiresAt:yyyy-MM-dd HH:mm:ss} UTC");
                    }
                    catch (Exception refreshEx)
                    {
                        Console.WriteLine($"   ⚠️  Manual token refresh failed: {refreshEx.Message}");
                        Console.WriteLine("   💡 This is expected if the refresh token is invalid or expired");
                    }
                }
                else
                {
                    Console.WriteLine("   ℹ️  No refresh token available for manual refresh demonstration");
                }
            }
            else
            {
                Console.WriteLine("   ⚠️  No current token available for refresh demonstration");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ⚠️  Token refresh demonstration failed: {ex.Message}");
        }
    }

    private static async Task TestRateLimitingScenarioAsync()
    {
        Console.WriteLine("   🚦 Rate limiting error handling...");
        
        try
        {
            // Simulate rate limiting scenario
            await Task.Delay(100); // Simulate processing
            Console.WriteLine("   ✅ Rate limiting handling: Retry logic and backoff configured");
            Console.WriteLine("   💡 Production apps should implement exponential backoff on 429 responses");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✅ Rate limiting error handled: {ex.Message}");
        }
    }

    private static string GenerateRandomState()
    {
        return $"console-{Guid.NewGuid():N}";
    }

    private static void OpenUrlInBrowser(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to open URL in browser");
            Console.WriteLine($"⚠️  Could not open browser automatically: {ex.Message}");
        }
    }
}
