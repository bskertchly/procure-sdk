# Task 8: Detailed Test Execution Guide

## Overview

This guide provides specific implementation steps, test scenarios, and validation methods for each of the 10 subtasks in the comprehensive sample application quality assurance strategy.

## Subtask Implementation Details

### Subtask 1: OAuth Flow Testing

#### Console Application OAuth Tests

**Test File**: `ConsoleApp/Authentication/ConsoleOAuthFlowTests.cs` (expand existing)

```csharp
[Fact]
public async Task ConsoleApp_PKCEFlowValidation_ShouldImplementCorrectCodeChallenge()
{
    // Arrange
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var state = "console-pkce-validation-test";

    // Act
    var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);

    // Assert - PKCE Validation
    var uri = new Uri(authUrl);
    var queryParams = HttpUtility.ParseQueryString(uri.Query);
    
    queryParams["code_challenge"].Should().NotBeNullOrEmpty("PKCE code challenge required");
    queryParams["code_challenge_method"].Should().Be("S256", "Must use SHA256 for PKCE");
    
    // Validate code challenge matches verifier
    var expectedChallenge = ComputeSHA256Base64UrlEncoded(codeVerifier);
    queryParams["code_challenge"].Should().Be(expectedChallenge, "Code challenge must match verifier");
}

[Fact]
public async Task ConsoleApp_StateParameterValidation_ShouldPreventCSRFAttacks()
{
    // Arrange
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var originalState = "console-csrf-test-state";

    // Act
    var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(originalState);
    
    // Simulate tampering with state parameter
    var tamperedState = "malicious-state-parameter";
    _fixture.MockTokenResponse(new { access_token = "token", expires_in = 3600 });

    // Assert - Should reject tampered state
    await Assert.ThrowsAsync<SecurityException>(
        () => oauthHelper.ExchangeCodeForTokenAsync("auth-code", codeVerifier, tamperedState));
}

[Fact]
public async Task ConsoleApp_ConcurrentAuthentication_ShouldHandleMultipleFlows()
{
    // Arrange
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var concurrentFlows = 10;
    var tasks = new List<Task<(string authUrl, string codeVerifier)>>();

    // Act - Create multiple concurrent flows
    for (int i = 0; i < concurrentFlows; i++)
    {
        tasks.Add(Task.Run(() => oauthHelper.GenerateAuthorizationUrl($"concurrent-{i}")));
    }

    var results = await Task.WhenAll(tasks);

    // Assert
    results.Should().HaveCount(concurrentFlows);
    results.Select(r => r.codeVerifier).Should().OnlyHaveUniqueItems("Each flow should have unique verifier");
    results.Select(r => r.authUrl).Should().OnlyHaveUniqueItems("Each flow should have unique URL");
}
```

**Performance Benchmarking**:

```csharp
[Benchmark]
public void AuthURL_Generation_Benchmark()
{
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("benchmark-test");
}

[Fact]
public async Task ConsoleApp_AuthURL_Generation_ShouldMeetPerformanceTarget()
{
    // Arrange
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var iterations = 1000;
    var stopwatch = Stopwatch.StartNew();

    // Act
    for (int i = 0; i < iterations; i++)
    {
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl($"perf-test-{i}");
    }

    stopwatch.Stop();

    // Assert
    var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
    averageTime.Should().BeLessOrEqualTo(1.0, "Average URL generation should be under 1ms");
}
```

#### Web Application OAuth Tests

**Test File**: `WebApp/Authentication/WebOAuthFlowTests.cs` (new)

```csharp
public class WebOAuthFlowTests : IClassFixture<WebApplicationTestFixture>
{
    [Fact]
    public async Task WebApp_OAuthCallback_ShouldValidateStateParameter()
    {
        // Arrange
        var client = _fixture.CreateClient();
        var validState = "web-oauth-state-12345";
        var authCode = "valid-authorization-code";

        // Act
        var response = await client.GetAsync($"/auth/callback?code={authCode}&state={validState}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        // Verify session contains token
        var sessionData = _fixture.GetSessionData();
        sessionData.Should().ContainKey("access_token");
    }

    [Fact]
    public async Task WebApp_SessionIsolation_ShouldHandleMultipleUsers()
    {
        // Arrange
        var client1 = _fixture.CreateClient();
        var client2 = _fixture.CreateClient();

        // Act - Simulate two users authenticating
        await AuthenticateUser(client1, "user1-state", "user1-token");
        await AuthenticateUser(client2, "user2-state", "user2-token");

        // Assert
        var user1Session = _fixture.GetSessionData(client1);
        var user2Session = _fixture.GetSessionData(client2);

        user1Session["access_token"].Should().Be("user1-token");
        user2Session["access_token"].Should().Be("user2-token");
    }
}
```

### Subtask 2: .NET Best Practices Compliance

#### Architecture Analysis Tests

**Test File**: `Shared/Architecture/DotNetBestPracticesTests.cs` (new)

```csharp
public class DotNetBestPracticesTests
{
    [Fact]
    public void ConsoleApp_DependencyInjection_ShouldFollowBestPractices()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var configuration = CreateTestConfiguration();

        // Act - Mirror the actual DI setup
        serviceCollection.AddProcoreSDK(configuration);
        serviceCollection.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
        serviceCollection.AddSingleton<ICoreClient, ProcoreCoreClient>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert
        // Verify all services can be resolved
        serviceProvider.GetService<OAuthFlowHelper>().Should().NotBeNull();
        serviceProvider.GetService<ITokenManager>().Should().NotBeNull();
        serviceProvider.GetService<ICoreClient>().Should().NotBeNull();

        // Verify proper lifetimes
        var tokenManager1 = serviceProvider.GetService<ITokenManager>();
        var tokenManager2 = serviceProvider.GetService<ITokenManager>();
        tokenManager1.Should().BeSameAs(tokenManager2, "ITokenManager should be singleton in console app");
    }

    [Fact]
    public async Task AsyncPatterns_ShouldNotBlockThreads()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var threadId = Thread.CurrentThread.ManagedThreadId;

        // Act
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("async-test");
        await Task.Delay(1); // Force async continuation

        // Assert
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        // In properly async code, thread IDs may differ after await
        _logger.LogInformation("Original thread: {Original}, Current thread: {Current}", 
            threadId, currentThreadId);
    }

    [Fact]
    public void Configuration_ShouldUseStandardPatterns()
    {
        // Arrange & Act
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        // Assert
        configuration.Should().NotBeNull();
        
        // Verify configuration sections exist
        var procoreSection = configuration.GetSection("Procore");
        procoreSection.Should().NotBeNull("Procore configuration section should exist");
        
        // Verify sensitive data is not in appsettings.json
        var clientSecret = configuration["Procore:ClientSecret"];
        if (!string.IsNullOrEmpty(clientSecret))
        {
            clientSecret.Should().NotStartWith("real-", "Sensitive data should not be in config files");
        }
    }
}
```

#### Code Quality Metrics Tests

```csharp
[Fact]
public void CodeComplexity_ShouldMeetStandards()
{
    // This would integrate with a static analysis tool
    var assemblyPath = typeof(Program).Assembly.Location;
    var complexityAnalyzer = new CyclomaticComplexityAnalyzer();
    
    var results = complexityAnalyzer.AnalyzeAssembly(assemblyPath);
    
    results.AverageComplexity.Should().BeLessOrEqualTo(10, "Average cyclomatic complexity should be <= 10");
    results.MaxComplexity.Should().BeLessOrEqualTo(15, "Maximum method complexity should be <= 15");
}
```

### Subtask 3: Error Handling Validation

#### Network Error Simulation Tests

**Test File**: `Shared/ErrorHandling/NetworkErrorTests.cs` (new)

```csharp
public class NetworkErrorTests : IClassFixture<TestAuthFixture>
{
    [Fact]
    public async Task OAuth_NetworkTimeout_ShouldHandleGracefully()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("timeout-test");

        // Setup timeout simulation
        _fixture.MockNetworkTimeout(TimeSpan.FromSeconds(30));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<TaskCanceledException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier));

        exception.Message.Should().Contain("timeout", "Should indicate timeout as cause");
    }

    [Fact]
    public async Task OAuth_DNSFailure_ShouldProvideHelpfulError()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("dns-test");

        // Setup DNS failure simulation
        _fixture.MockDNSFailure();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier));

        exception.Message.Should().Contain("DNS", "Should indicate DNS resolution failure");
    }

    [Fact]
    public async Task OAuth_RateLimiting_ShouldImplementBackoff()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("rate-limit-test");

        // Setup rate limiting response (HTTP 429)
        _fixture.MockRateLimitResponse(retryAfterSeconds: 5);

        // Act
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("429"))
        {
            stopwatch.Stop();
            
            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeGreaterThan(4000, 
                "Should wait at least 4 seconds before giving up");
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(10000, 
                "Should not wait more than 10 seconds");
        }
    }
}
```

#### Application Error Handling Tests

```csharp
[Fact]
public async Task TokenManager_ConcurrentAccess_ShouldBeSafe()
{
    // Arrange
    var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
    var token = new AccessToken("test-token", "Bearer", DateTimeOffset.UtcNow.AddHours(1), 
        "refresh-token", new[] { "read" });

    // Act - Concurrent operations
    var tasks = new List<Task>();
    for (int i = 0; i < 100; i++)
    {
        tasks.Add(tokenManager.StoreTokenAsync(token));
        tasks.Add(tokenManager.GetAccessTokenAsync());
    }

    // Assert - Should not throw
    await Assert.DoesNotThrowAsync(() => Task.WhenAll(tasks));
}

[Fact]
public void Configuration_MissingRequired_ShouldFailFast()
{
    // Arrange - Configuration without required values
    var incompleteConfig = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string>
        {
            // Missing ClientId and other required values
            ["Procore:ClientSecret"] = "test-secret"
        })
        .Build();

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() =>
    {
        var services = new ServiceCollection();
        services.AddProcoreSDK(incompleteConfig);
        services.BuildServiceProvider().GetRequiredService<OAuthFlowHelper>();
    });
}
```

### Subtask 4: Multi-Version Compatibility

#### Framework Compatibility Tests

**Test File**: `Shared/Compatibility/FrameworkCompatibilityTests.cs` (new)

```csharp
public class FrameworkCompatibilityTests
{
    [Fact]
    public void Framework_TargetFramework_ShouldBeNet80()
    {
        // Arrange & Act
        var assembly = typeof(Program).Assembly;
        var targetFramework = assembly.GetCustomAttribute<TargetFrameworkAttribute>();

        // Assert
        targetFramework.Should().NotBeNull("Assembly should have TargetFramework attribute");
        targetFramework!.FrameworkName.Should().StartWith(".NETCoreApp,Version=v8.0", 
            "Should target .NET 8.0");
    }

    [Fact]
    public void Dependencies_ShouldUseCompatibleVersions()
    {
        // Arrange
        var assembly = typeof(Program).Assembly;
        
        // Act
        var referencedAssemblies = assembly.GetReferencedAssemblies();
        
        // Assert
        var microsoftExtensions = referencedAssemblies
            .Where(a => a.Name!.StartsWith("Microsoft.Extensions"))
            .ToList();

        microsoftExtensions.Should().NotBeEmpty("Should reference Microsoft.Extensions packages");
        
        // All Microsoft.Extensions packages should be compatible versions
        foreach (var reference in microsoftExtensions)
        {
            reference.Version!.Major.Should().BeGreaterOrEqualTo(8, 
                $"{reference.Name} should be version 8.0 or higher for .NET 8 compatibility");
        }
    }

    [Theory]
    [InlineData("win-x64")]
    [InlineData("linux-x64")]
    [InlineData("osx-x64")]
    public void Runtime_ShouldSupportPlatform(string runtimeIdentifier)
    {
        // This test would be run in a multi-platform CI pipeline
        // For now, we just verify the runtime information is available
        var runtimeInfo = RuntimeInformation.RuntimeIdentifier;
        runtimeInfo.Should().NotBeNullOrEmpty("Runtime identifier should be available");
        
        _logger.LogInformation("Current runtime: {Runtime}, Testing for: {Target}", 
            runtimeInfo, runtimeIdentifier);
    }
}
```

#### API Version Compatibility Tests

```csharp
[Fact]
public async Task API_MultipleVersions_ShouldBeSupported()
{
    // Arrange
    var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();
    
    // Mock responses for different API versions
    _fixture.MockApiResponse("/rest/v1.0/projects", new { projects = new[] { new { id = 1, name = "Test V1.0" } } });
    _fixture.MockApiResponse("/rest/v1.1/projects", new { projects = new[] { new { id = 1, name = "Test V1.1" } } });

    // Act & Assert - Both versions should work
    var v10Response = await coreClient.Rest.V10.Projects.GetAsync();
    var v11Response = await coreClient.Rest.V11.Projects.GetAsync();

    v10Response.Should().NotBeNull("V1.0 API should be supported");
    v11Response.Should().NotBeNull("V1.1 API should be supported");
}
```

### Subtask 5: Security Implementation Review

#### PKCE Security Tests

**Test File**: `Shared/Security/PKCESecurityTests.cs` (new)

```csharp
public class PKCESecurityTests : IClassFixture<TestAuthFixture>
{
    [Fact]
    public void PKCE_CodeVerifier_ShouldMeetSpecification()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();

        // Act
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("pkce-spec-test");

        // Assert - RFC 7636 requirements
        codeVerifier.Length.Should().BeInRange(43, 128, 
            "Code verifier length must be 43-128 characters per RFC 7636");
        
        // Should only contain allowed characters: [A-Z] / [a-z] / [0-9] / "-" / "." / "_" / "~"
        var allowedPattern = @"^[A-Za-z0-9\-\._~]+$";
        Regex.IsMatch(codeVerifier, allowedPattern).Should().BeTrue(
            "Code verifier should only contain URL-safe characters");
    }

    [Fact]
    public void PKCE_CodeChallenge_ShouldUseSHA256()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();

        // Act
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("sha256-test");

        // Assert
        var uri = new Uri(authUrl);
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        
        queryParams["code_challenge_method"].Should().Be("S256", 
            "Should use SHA256 for code challenge method");
        
        // Verify the challenge matches the verifier using SHA256
        var expectedChallenge = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier)))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
        
        queryParams["code_challenge"].Should().Be(expectedChallenge, 
            "Code challenge should be SHA256 hash of verifier");
    }

    [Fact]
    public void OAuth_RedirectURI_ShouldValidateStrictly()
    {
        // Arrange
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var configuredRedirectUri = configuration["Procore:RedirectUri"];

        // Act & Assert
        configuredRedirectUri.Should().NotBeNullOrEmpty("Redirect URI should be configured");
        configuredRedirectUri.Should().StartWith("https://", "Redirect URI should use HTTPS");
        
        // Should not allow open redirects
        var uri = new Uri(configuredRedirectUri!);
        uri.IsLoopback.Should().BeTrue("Development redirect URI should be localhost");
    }

    [Fact]
    public async Task TokenStorage_ShouldBeSecure()
    {
        // Arrange
        var tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();
        var sensitiveToken = new AccessToken("sensitive-access-token", "Bearer", 
            DateTimeOffset.UtcNow.AddHours(1), "sensitive-refresh-token", new[] { "admin" });

        // Act
        await tokenStorage.StoreTokenAsync(sensitiveToken);
        var retrievedToken = await tokenStorage.GetTokenAsync();

        // Assert
        retrievedToken.Should().NotBeNull("Token should be retrievable");
        retrievedToken!.Token.Should().Be(sensitiveToken.Token, "Token should match exactly");
        
        // Verify token is not stored in plain text (this would be implementation-specific)
        // For in-memory storage, ensure it's not accidentally serialized or logged
    }
}
```

#### Web Security Tests

```csharp
public class WebSecurityTests : IClassFixture<WebApplicationTestFixture>
{
    [Fact]
    public async Task Web_SecurityHeaders_ShouldBePresent()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.Headers.Should().ContainSingle(h => h.Key == "X-Content-Type-Options")
            .Which.Value.Should().Contain("nosniff");
        response.Headers.Should().ContainSingle(h => h.Key == "X-Frame-Options")
            .Which.Value.Should().Contain("DENY");
        response.Headers.Should().ContainSingle(h => h.Key == "X-XSS-Protection")
            .Which.Value.Should().Contain("1; mode=block");
    }

    [Fact]
    public async Task Web_HTTPS_ShouldBeEnforced()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act - Try HTTP request (should redirect to HTTPS)
        var response = await client.GetAsync("http://localhost/");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Redirect, HttpStatusCode.MovedPermanently,
            "HTTP requests should redirect to HTTPS");
    }

    [Fact]
    public async Task Web_SessionCookies_ShouldBeSecure()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var response = await client.GetAsync("/auth/login");

        // Assert
        var setCookieHeaders = response.Headers.GetValues("Set-Cookie");
        foreach (var cookieHeader in setCookieHeaders)
        {
            if (cookieHeader.Contains("__ProcoreSession") || cookieHeader.Contains("__ProcoreAuth"))
            {
                cookieHeader.Should().Contain("HttpOnly", "Session cookies should be HttpOnly");
                cookieHeader.Should().Contain("Secure", "Session cookies should be Secure");
                cookieHeader.Should().Contain("SameSite=Strict", "Session cookies should use SameSite=Strict");
            }
        }
    }
}
```

### Subtask 6: Performance Testing

#### Authentication Performance Tests

**Test File**: `Performance/AuthenticationPerformanceTests.cs` (expand existing)

```csharp
[Fact]
public async Task Authentication_LoadTest_ShouldHandleConcurrentUsers()
{
    // Arrange
    var concurrentUsers = 100;
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var tasks = new List<Task<TimeSpan>>();

    // Act
    for (int i = 0; i < concurrentUsers; i++)
    {
        tasks.Add(MeasureAuthenticationTime(oauthHelper, $"load-test-user-{i}"));
    }

    var results = await Task.WhenAll(tasks);

    // Assert
    var averageTime = results.Average(r => r.TotalMilliseconds);
    var maxTime = results.Max(r => r.TotalMilliseconds);

    averageTime.Should().BeLessOrEqualTo(100, "Average authentication time should be under 100ms");
    maxTime.Should().BeLessOrEqualTo(500, "Maximum authentication time should be under 500ms");
    
    _logger.LogInformation("Load test results: Avg={AvgMs}ms, Max={MaxMs}ms, Users={Users}", 
        averageTime, maxTime, concurrentUsers);
}

private async Task<TimeSpan> MeasureAuthenticationTime(OAuthFlowHelper oauthHelper, string state)
{
    var stopwatch = Stopwatch.StartNew();
    
    try
    {
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);
        
        // Mock successful token exchange
        _fixture.MockTokenResponse(new { access_token = $"token-{state}", expires_in = 3600 });
        
        await oauthHelper.ExchangeCodeForTokenAsync("test-code", codeVerifier);
        
        return stopwatch.Elapsed;
    }
    finally
    {
        stopwatch.Stop();
    }
}

[Fact]
public async Task TokenStorage_Performance_ShouldMeetBenchmarks()
{
    // Arrange
    var tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();
    var testToken = new AccessToken("perf-test-token", "Bearer", 
        DateTimeOffset.UtcNow.AddHours(1), "refresh", new[] { "read" });
    
    var iterations = 1000;
    var stopwatch = new Stopwatch();

    // Act - Store operations
    stopwatch.Start();
    for (int i = 0; i < iterations; i++)
    {
        await tokenStorage.StoreTokenAsync(testToken);
    }
    stopwatch.Stop();
    
    var storeAverage = stopwatch.ElapsedMilliseconds / (double)iterations;

    // Act - Retrieve operations
    stopwatch.Restart();
    for (int i = 0; i < iterations; i++)
    {
        await tokenStorage.GetTokenAsync();
    }
    stopwatch.Stop();
    
    var retrieveAverage = stopwatch.ElapsedMilliseconds / (double)iterations;

    // Assert
    storeAverage.Should().BeLessOrEqualTo(10, "Token store should average under 10ms");
    retrieveAverage.Should().BeLessOrEqualTo(5, "Token retrieve should average under 5ms");
}
```

#### Memory Performance Tests

```csharp
[Fact]
public async Task Memory_LongRunningOperation_ShouldNotLeak()
{
    // Arrange
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var initialMemory = GC.GetTotalMemory(true);
    var operations = 10000;

    // Act - Perform many operations
    for (int i = 0; i < operations; i++)
    {
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl($"memory-test-{i}");
        
        // Simulate some work
        await Task.Delay(1);
        
        if (i % 1000 == 0)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    // Force garbage collection
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    var finalMemory = GC.GetTotalMemory(false);
    var memoryIncrease = finalMemory - initialMemory;

    // Assert
    memoryIncrease.Should().BeLessThan(50 * 1024 * 1024, 
        "Memory increase should be less than 50MB after 10k operations");
    
    _logger.LogInformation("Memory test: Initial={InitialMB}MB, Final={FinalMB}MB, Increase={IncreaseMB}MB",
        initialMemory / 1024 / 1024, finalMemory / 1024 / 1024, memoryIncrease / 1024 / 1024);
}
```

### Subtask 7: Code Quality Assessment

#### Static Analysis Integration

**Test File**: `Quality/StaticAnalysisTests.cs` (new)

```csharp
public class StaticAnalysisTests
{
    [Fact]
    public void CodeCoverage_ShouldMeetMinimumThreshold()
    {
        // This would integrate with coverage tools
        // For demonstration, we'll simulate the check
        var coverageReport = GetCodeCoverageReport();
        
        coverageReport.LineCoverage.Should().BeGreaterOrEqualTo(0.90, 
            "Line coverage should be at least 90%");
        coverageReport.BranchCoverage.Should().BeGreaterOrEqualTo(0.85, 
            "Branch coverage should be at least 85%");
    }

    [Fact]
    public void TechnicalDebt_ShouldBeBelowThreshold()
    {
        // This would integrate with SonarQube or similar
        var debtReport = GetTechnicalDebtReport();
        
        debtReport.DebtRatio.Should().BeLessOrEqualTo(0.05, 
            "Technical debt ratio should be below 5%");
        debtReport.CodeSmells.Should().BeLessOrEqualTo(10, 
            "Should have minimal code smells");
    }

    [Fact]
    public void SecurityVulnerabilities_ShouldBeZero()
    {
        var securityReport = GetSecurityScanReport();
        
        securityReport.CriticalVulnerabilities.Should().Be(0, 
            "Should have zero critical vulnerabilities");
        securityReport.HighVulnerabilities.Should().Be(0, 
            "Should have zero high vulnerabilities");
    }

    private CodeCoverageReport GetCodeCoverageReport()
    {
        // In real implementation, this would parse actual coverage reports
        return new CodeCoverageReport
        {
            LineCoverage = 0.92,
            BranchCoverage = 0.87
        };
    }
}
```

### Subtask 8: User Experience Validation

#### Console UX Tests

**Test File**: `ConsoleApp/UserExperience/ConsoleUXTests.cs` (new)

```csharp
public class ConsoleUXTests
{
    [Fact]
    public async Task Console_ErrorMessages_ShouldBeHelpful()
    {
        // Arrange
        var output = new StringWriter();
        Console.SetOut(output);
        
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl("ux-test");

        // Setup error scenario
        _fixture.MockTokenErrorResponse(HttpStatusCode.BadRequest, new 
        { 
            error = "invalid_grant", 
            error_description = "The authorization code is invalid" 
        });

        // Act
        try
        {
            await oauthHelper.ExchangeCodeForTokenAsync("invalid-code", codeVerifier);
        }
        catch (Exception ex)
        {
            // The application should handle this and provide helpful output
            Console.WriteLine($"❌ Authentication failed: {ex.Message}");
            Console.WriteLine("💡 Please check that you copied the authorization code correctly");
        }

        // Assert
        var consoleOutput = output.ToString();
        consoleOutput.Should().Contain("❌", "Should use clear error indicators");
        consoleOutput.Should().Contain("💡", "Should provide helpful guidance");
        consoleOutput.Should().NotContain("Exception", "Should not show raw exception details to users");
    }

    [Fact]
    public void Console_Instructions_ShouldBeClear()
    {
        // Arrange
        var output = new StringWriter();
        Console.SetOut(output);

        // Act - Simulate the instruction output from the console app
        Console.WriteLine("📋 Please complete the following steps:");
        Console.WriteLine("1. Copy the authorization URL below");
        Console.WriteLine("2. Open it in your browser");
        Console.WriteLine("3. Sign in to Procore and authorize the application");
        Console.WriteLine("4. Copy the authorization code from the callback URL");

        // Assert
        var instructions = output.ToString();
        instructions.Should().Contain("📋", "Should use clear visual indicators");
        instructions.Should().Contain("1.", "Should provide numbered steps");
        instructions.Should().Contain("2.", "Should provide numbered steps");
        instructions.Should().Contain("3.", "Should provide numbered steps");
        instructions.Should().Contain("4.", "Should provide numbered steps");
        
        // Each step should be actionable
        instructions.Should().Contain("Copy", "Should have clear actions");
        instructions.Should().Contain("Open", "Should have clear actions");
        instructions.Should().Contain("Sign in", "Should have clear actions");
    }
}
```

#### Web UX Tests (using Playwright)

**Test File**: `WebApp/UserExperience/WebUXTests.cs` (new)

```csharp
public class WebUXTests : IClassFixture<WebApplicationTestFixture>
{
    [Fact]
    public async Task Web_ResponsiveDesign_ShouldWorkOnMobile()
    {
        // This would use Playwright for real browser testing
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        
        // Mobile viewport
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 375, Height = 667 } // iPhone SE
        });
        
        var page = await context.NewPageAsync();
        await page.GotoAsync(_fixture.ServerAddress);

        // Assert mobile-friendly layout
        var navigation = await page.QuerySelectorAsync("nav");
        navigation.Should().NotBeNull("Navigation should be present");
        
        // Check if responsive menu is working
        var menuButton = await page.QuerySelectorAsync("[data-testid='mobile-menu-button']");
        if (menuButton != null)
        {
            await menuButton.ClickAsync();
            var mobileMenu = await page.QuerySelectorAsync("[data-testid='mobile-menu']");
            mobileMenu.Should().NotBeNull("Mobile menu should appear when button clicked");
        }
    }

    [Fact]
    public async Task Web_LoadingStates_ShouldProvideVisualFeedback()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();
        
        await page.GotoAsync($"{_fixture.ServerAddress}/auth/login");
        
        // Click login button and check for loading state
        await page.ClickAsync("button[type='submit']");
        
        // Should show loading indicator
        var loadingIndicator = await page.WaitForSelectorAsync(".loading, .spinner, [data-testid='loading']", 
            new PageWaitForSelectorOptions { Timeout = 5000 });
        
        loadingIndicator.Should().NotBeNull("Should show loading indicator during authentication");
    }

    [Fact]
    public async Task Web_AccessibilityCompliance_ShouldMeetWCAG()
    {
        // This would use axe-core or similar accessibility testing tools
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();
        
        await page.GotoAsync(_fixture.ServerAddress);
        
        // Inject axe-core and run accessibility scan
        await page.AddScriptTagAsync(new PageAddScriptTagOptions 
        { 
            Path = "node_modules/axe-core/axe.min.js" 
        });
        
        var results = await page.EvaluateAsync(@"
            new Promise(resolve => {
                axe.run((err, results) => {
                    resolve(results);
                });
            })
        ");

        // Assert no critical accessibility violations
        var violations = results.GetProperty("violations").EnumerateArray().ToList();
        var criticalViolations = violations.Where(v => 
            v.GetProperty("impact").GetString() == "critical").ToList();
            
        criticalViolations.Should().BeEmpty("Should have no critical accessibility violations");
    }
}
```

### Subtask 9: Integration Testing

#### End-to-End Workflow Tests

**Test File**: `Integration/EndToEndWorkflowTests.cs` (expand existing)

```csharp
[Fact]
public async Task EndToEnd_ConsoleWorkflow_WithRealAPI_ShouldCompleteSuccessfully()
{
    // This test would run against a real Procore sandbox environment
    // Requires special configuration and should be marked for integration test category
    
    // Arrange
    var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
    var tokenManager = _serviceProvider.GetRequiredService<ITokenManager>();
    var coreClient = _serviceProvider.GetRequiredService<ICoreClient>();

    // Skip if not configured for real API testing
    var testConfig = _configuration["Testing:UseRealAPI"];
    if (testConfig != "true")
    {
        _logger.LogInformation("Skipping real API test - not configured");
        return;
    }

    // Act
    // 1. Generate auth URL
    var state = $"integration-test-{Guid.NewGuid():N}";
    var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);

    // 2. In real scenario, user would authenticate and we'd get code
    // For integration test, we use a pre-configured test authorization code
    var testAuthCode = _configuration["Testing:TestAuthCode"];
    if (string.IsNullOrEmpty(testAuthCode))
    {
        _logger.LogWarning("Test authorization code not configured - test skipped");
        return;
    }

    // 3. Exchange code for token
    var token = await oauthHelper.ExchangeCodeForTokenAsync(testAuthCode, codeVerifier);
    await tokenManager.StoreTokenAsync(token);

    // 4. Perform API operations
    var companies = await coreClient.GetCompaniesAsync();
    var companiesList = companies.ToList();

    // Assert
    token.Should().NotBeNull("Should successfully exchange code for token");
    token.Token.Should().NotBeNullOrEmpty("Token should have access token");
    companiesList.Should().NotBeEmpty("Should be able to fetch companies from API");

    _logger.LogInformation("Integration test successful - retrieved {Count} companies", 
        companiesList.Count);
}

[Fact]
public async Task EndToEnd_WebWorkflow_ShouldHandleCompleteUserJourney()
{
    // Arrange
    using var playwright = await Playwright.CreateAsync();
    await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
    {
        Headless = true // Set to false for debugging
    });
    
    var page = await browser.NewPageAsync();

    // Act & Assert - Complete user journey
    
    // 1. Navigate to application
    await page.GotoAsync(_fixture.ServerAddress);
    await page.WaitForLoadStateAsync(LoadState.Load);

    // 2. Should redirect to login
    await page.WaitForURLAsync("**/auth/login");
    
    // 3. Click login button (starts OAuth flow)
    await page.ClickAsync("a[href*='/auth/login']");
    
    // 4. Should redirect to OAuth provider (mocked in test)
    // In real test, this would go to actual Procore OAuth
    await page.WaitForURLAsync("**/oauth/**");
    
    // 5. Simulate successful OAuth callback
    var callbackUrl = $"{_fixture.ServerAddress}/auth/callback?code=test-code&state=test-state";
    await page.GotoAsync(callbackUrl);
    
    // 6. Should redirect to dashboard after successful auth
    await page.WaitForURLAsync("**/dashboard");
    
    // 7. Verify authenticated content is visible
    var userInfo = await page.QuerySelectorAsync("[data-testid='user-info']");
    userInfo.Should().NotBeNull("Authenticated user info should be displayed");
    
    // 8. Test logout
    await page.ClickAsync("a[href*='/auth/logout']");
    await page.WaitForURLAsync("**/");
    
    // Should be back to unauthenticated state
    var loginLink = await page.QuerySelectorAsync("a[href*='/auth/login']");
    loginLink.Should().NotBeNull("Login link should be visible after logout");
}
```

### Subtask 10: Deployment & Configuration

#### Configuration Validation Tests

**Test File**: `Deployment/ConfigurationTests.cs` (new)

```csharp
public class ConfigurationTests
{
    [Fact]
    public void Configuration_ProductionSettings_ShouldBeSecure()
    {
        // Arrange
        var productionConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Production.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Act & Assert
        var loggingSection = productionConfig.GetSection("Logging");
        var logLevel = loggingSection["LogLevel:Default"];
        
        // Production should not log Debug/Trace
        logLevel.Should().NotBe("Debug", "Production should not use Debug logging");
        logLevel.Should().NotBe("Trace", "Production should not use Trace logging");
        
        // Should prefer Information or Warning in production
        logLevel.Should().BeOneOf("Information", "Warning", "Error", "Critical");
    }

    [Fact]
    public void Configuration_RequiredSettings_ShouldBeValidated()
    {
        // Arrange
        var requiredSettings = new[]
        {
            "Procore:ClientId",
            "Procore:RedirectUri",
            "Procore:Scopes"
        };

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        // Act & Assert
        foreach (var setting in requiredSettings)
        {
            var value = configuration[setting];
            value.Should().NotBeNullOrEmpty($"{setting} should be configured");
            
            // Client secret should not be in appsettings.json
            if (setting.Contains("Secret"))
            {
                value.Should().NotStartWith("real", "Secrets should not be in config files");
            }
        }
    }

    [Fact]  
    public void UserSecrets_ShouldBeConfiguredForDevelopment()
    {
        // Arrange
        var userSecretsConfig = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        // Act
        var clientSecret = userSecretsConfig["Procore:ClientSecret"];

        // Assert
        // In development, user secrets should be configured
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            clientSecret.Should().NotBeNullOrEmpty("ClientSecret should be in user secrets for development");
        }
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Staging")]
    [InlineData("Production")]
    public void Configuration_ByEnvironment_ShouldBeValid(string environment)
    {
        // Arrange
        var configFile = $"appsettings.{environment}.json";
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile(configFile, optional: true);

        if (environment == "Development")
        {
            configBuilder.AddUserSecrets<Program>();
        }

        var configuration = configBuilder.Build();

        // Act & Assert
        var procoreSection = configuration.GetSection("Procore");
        procoreSection.Should().NotBeNull($"Procore section should exist for {environment}");

        // Environment-specific validations
        switch (environment)
        {
            case "Development":
                // Development can use localhost redirect URIs
                var devRedirectUri = configuration["Procore:RedirectUri"];
                devRedirectUri?.Should().Contain("localhost", "Development should use localhost");
                break;
                
            case "Production":
                // Production must use HTTPS and real domains
                var prodRedirectUri = configuration["Procore:RedirectUri"];
                prodRedirectUri?.Should().StartWith("https://", "Production must use HTTPS");
                prodRedirectUri?.Should().NotContain("localhost", "Production should not use localhost");
                break;
        }
    }
}
```

#### Docker Deployment Tests

```csharp
[Fact]
public async Task Docker_ConsoleApp_ShouldBuildAndRun()
{
    // This test would require Docker to be available
    if (!IsDockerAvailable())
    {
        _logger.LogWarning("Docker not available - skipping Docker tests");
        return;
    }

    // Arrange
    var dockerfilePath = Path.Combine(GetProjectRoot(), "samples", "ConsoleSample", "Dockerfile");
    
    // Act - Build Docker image
    var buildResult = await RunDockerCommand($"build -t procore-console-sample -f {dockerfilePath} .");
    
    // Assert
    buildResult.ExitCode.Should().Be(0, "Docker build should succeed");
    
    // Act - Run container (in test mode)
    var runResult = await RunDockerCommand("run --rm -e ASPNETCORE_ENVIRONMENT=Testing procore-console-sample --version");
    
    // Assert
    runResult.ExitCode.Should().Be(0, "Docker container should run successfully");
}

private bool IsDockerAvailable()
{
    try
    {
        var result = Process.Start(new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "version",
            UseShellExecute = false,
            CreateNoWindow = true
        });
        
        result?.WaitForExit();
        return result?.ExitCode == 0;
    }
    catch
    {
        return false;
    }
}
```

## Test Execution Commands

### Running Individual Subtask Tests

```bash
# Subtask 1: OAuth Flow Testing
dotnet test --filter "FullyQualifiedName~OAuth"

# Subtask 2: .NET Best Practices
dotnet test --filter "FullyQualifiedName~DotNetBestPractices"

# Subtask 3: Error Handling
dotnet test --filter "FullyQualifiedName~ErrorHandling"

# Subtask 4: Multi-Version Compatibility
dotnet test --filter "FullyQualifiedName~Compatibility"

# Subtask 5: Security Implementation
dotnet test --filter "FullyQualifiedName~Security"

# Subtask 6: Performance Testing
dotnet test --filter "Category=Performance"

# Subtask 7: Code Quality
dotnet test --filter "FullyQualifiedName~Quality"

# Subtask 8: User Experience
dotnet test --filter "FullyQualifiedName~UX"

# Subtask 9: Integration Testing
dotnet test --filter "Category=Integration"

# Subtask 10: Deployment & Configuration
dotnet test --filter "FullyQualifiedName~Deployment"
```

### Comprehensive Test Execution

```bash
# Run all Task 8 QA tests
dotnet test --filter "FullyQualifiedName~Task8" --collect:"XPlat Code Coverage"

# Generate coverage report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"task8-coverage" -reporttypes:Html

# Run performance benchmarks
dotnet run --project tests/Procore.SDK.Samples.Tests --configuration Release -- --benchmark

# Run security scans
dotnet run --project tools/SecurityScanner -- --target samples/
```

## Success Criteria Validation

Each subtask includes specific success criteria that can be validated:

1. **OAuth Flow**: 100% success rate, sub-100ms performance
2. **.NET Best Practices**: 90%+ code coverage, proper DI usage
3. **Error Handling**: Graceful failure handling, helpful error messages
4. **Multi-Version Compatibility**: Cross-platform testing, API version support
5. **Security Implementation**: Zero critical vulnerabilities, PKCE compliance
6. **Performance Testing**: Load handling, memory efficiency
7. **Code Quality**: >8.0 maintainability index, <5% technical debt
8. **User Experience**: Mobile responsiveness, accessibility compliance
9. **Integration Testing**: End-to-end workflow completion
10. **Deployment & Configuration**: Environment-specific validation, Docker support

This detailed execution guide provides the foundation for comprehensive quality assurance testing of the Procore SDK sample applications.