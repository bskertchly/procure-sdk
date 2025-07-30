# Procore SDK Integration Test Troubleshooting Guide

## Overview

This guide provides comprehensive troubleshooting information for common issues encountered during integration testing, setup, and development with the Procore SDK for .NET.

## Common Integration Test Issues

### Authentication Issues

#### Problem: OAuth Authentication Failing
```
Error: No valid authentication token available
Error: AuthenticationException: Invalid client credentials
```

**Solutions:**
1. **Verify Sandbox Credentials**:
   ```bash
   # Check user secrets are configured
   dotnet user-secrets list --project tests/Procore.SDK.IntegrationTests.Live
   
   # Set correct credentials
   dotnet user-secrets set "Procore:ClientId" "your_sandbox_client_id"
   dotnet user-secrets set "Procore:ClientSecret" "your_sandbox_client_secret"
   ```

2. **Validate OAuth Application Settings**:
   - Ensure redirect URI matches configuration
   - Verify scopes are correctly configured
   - Check sandbox application is active

3. **Token Storage Issues**:
   ```bash
   # Clear existing tokens
   rm -f integration-test-tokens.json
   
   # Re-run authentication setup
   dotnet test --filter "Category=Integration&Focus=Authentication" --logger "console;verbosity=detailed"
   ```

#### Problem: Token Refresh Failures
```
Error: Refresh token expired or invalid
Error: TokenRefreshException: Unable to refresh access token
```

**Solutions:**
1. **Check Token Expiration**:
   - Verify token refresh margin settings
   - Ensure refresh tokens are being stored properly
   - Check system clock synchronization

2. **Re-authenticate**:
   ```bash
   # Force re-authentication
   dotnet user-secrets set "Procore:ForceReauth" "true"
   dotnet test --filter "Focus=Authentication"
   ```

### API Connectivity Issues

#### Problem: Network Connectivity Errors
```
Error: HttpRequestException: No such host is known
Error: TaskCanceledException: The operation was canceled
```

**Solutions:**
1. **Verify Network Connectivity**:
   ```bash
   # Test connectivity to Procore sandbox
   curl -I https://sandbox.procore.com
   ping sandbox.procore.com
   ```

2. **Check Proxy/Firewall Settings**:
   ```bash
   # Set proxy if required
   export HTTPS_PROXY=http://your-proxy:port
   export HTTP_PROXY=http://your-proxy:port
   ```

3. **Timeout Configuration**:
   ```json
   {
     "TestSettings": {
       "TimeoutSeconds": 60,
       "MaxRetries": 5
     }
   }
   ```

#### Problem: SSL/TLS Certificate Issues
```
Error: The SSL connection could not be established
Error: Certificate validation failed
```

**Solutions:**
1. **Update Certificates**:
   ```bash
   # Update certificate store (Windows)
   certlm.msc
   
   # Update certificates (Linux/macOS)
   sudo apt-get update && sudo apt-get install ca-certificates
   ```

2. **Bypass SSL Validation (Development Only)**:
   ```csharp
   // Only for testing - never use in production
   ServiceCollection.Configure<HttpClientOptions>(options =>
   {
       options.HttpClientFactory = new TestHttpClientFactory(bypassSsl: true);
   });
   ```

### Performance Test Issues

#### Problem: Tests Timing Out
```
Error: Test execution timeout exceeded
Error: NBomber load test failed to complete
```

**Solutions:**
1. **Adjust Performance Thresholds**:
   ```json
   {
     "TestSettings": {
       "PerformanceThresholds": {
         "AuthenticationMs": 5000,
         "ApiOperationMs": 10000,
         "BulkOperationMs": 60000
       }
     }
   }
   ```

2. **Reduce Test Load**:
   ```csharp
   // Reduce concurrency for slower networks
   const int concurrencyLevel = 3; // Instead of 10
   ```

3. **Enable Debug Logging**:
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Procore.SDK": "Debug",
         "NBomber": "Debug"
       }
     }
   }
   ```

#### Problem: Rate Limiting (429 Errors)
```
Error: HTTP 429 - Too Many Requests
```

**Solutions:**
1. **Implement Exponential Backoff**:
   ```csharp
   // Rate limiting is handled automatically by resilience policies
   var resilientClient = fixture.CreateClientWithOptions<ProcoreCoreClient>(options =>
   {
       options.RetryAttempts = 5;
       options.RetryDelayMilliseconds = 1000;
       options.RetryBackoffMultiplier = 2.0;
   });
   ```

2. **Reduce Request Rate**:
   ```csharp
   // Add delays between requests
   await Task.Delay(500); // 500ms between requests
   ```

### Data Consistency Issues

#### Problem: Cross-Client Data Validation Failures
```
Error: Project data inconsistent across clients
Error: Referential integrity validation failed
```

**Solutions:**
1. **Check Data Cleanup**:
   ```csharp
   // Ensure test data is properly cleaned up
   [TearDown]
   public async Task CleanupAsync()
   {
       await fixture.CleanupAllTestDataAsync();
   }
   ```

2. **Add Data Synchronization Delays**:
   ```csharp
   // Allow time for data propagation
   await Task.Delay(1000);
   await ValidateDataConsistencyAsync();
   ```

3. **Verify Test Isolation**:
   ```csharp
   // Use unique identifiers for test data
   var uniqueId = $"test_{Guid.NewGuid():N}";
   ```

## Environment Setup Issues

### Development Environment

#### Problem: Missing Dependencies
```
Error: Could not load file or assembly 'Microsoft.Kiota'
Error: Package 'NBomber' not found
```

**Solutions:**
1. **Restore NuGet Packages**:
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. **Update Package References**:
   ```bash
   dotnet list package --outdated
   dotnet add package NBomber --version 5.0.0
   ```

#### Problem: Configuration Loading Issues
```
Error: Configuration value 'Procore:ClientId' not found
Error: IConfiguration binding failed
```

**Solutions:**
1. **Verify Configuration Files**:
   ```bash
   # Check appsettings files exist
   ls -la appsettings*.json
   
   # Validate JSON format
   cat appsettings.json | jq .
   ```

2. **Check User Secrets**:
   ```bash
   # Initialize user secrets if needed
   dotnet user-secrets init
   
   # List current secrets
   dotnet user-secrets list
   ```

### CI/CD Pipeline Issues

#### Problem: Tests Failing in CI/CD
```
Error: Tests pass locally but fail in pipeline
Error: Environment variables not loaded
```

**Solutions:**
1. **Environment Variable Configuration**:
   ```yaml
   # GitHub Actions example
   env:
     PROCORE_INTEGRATION_Procore__ClientId: ${{ secrets.PROCORE_CLIENT_ID }}
     PROCORE_INTEGRATION_Procore__ClientSecret: ${{ secrets.PROCORE_CLIENT_SECRET }}
     PROCORE_INTEGRATION_Procore__TestCompanyId: ${{ secrets.PROCORE_TEST_COMPANY_ID }}
   ```

2. **Test Categorization**:
   ```bash
   # Run only stable tests in CI
   dotnet test --filter "Category=Integration&Priority=High"
   
   # Skip slow tests in PR builds
   dotnet test --filter "Category=Integration&TestType!=LoadTesting"
   ```

3. **Timeout Configuration**:
   ```yaml
   # Increase timeouts for CI environment
   - name: Run Integration Tests
     run: dotnet test --logger trx --timeout 1800000 # 30 minutes
   ```

## Debugging Techniques

### Enable Detailed Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Procore.SDK": "Debug",
      "Microsoft.Extensions.Http": "Debug",
      "System.Net.Http.HttpClient": "Debug"
    }
  }
}
```

### Debug HTTP Traffic

```bash
# Enable HTTP logging
export DOTNET_HTTPLOGGING_ENABLED=true
export DOTNET_HTTPLOGGING_LOGLEVEL=Debug

# Use Fiddler or similar proxy
export HTTPS_PROXY=http://127.0.0.1:8888
export HTTP_PROXY=http://127.0.0.1:8888
```

### Performance Debugging

```csharp
// Add performance tracking to tests
using var activity = ActivitySource.StartActivity("TestOperation");
var stopwatch = Stopwatch.StartNew();

try
{
    await operation();
    activity?.SetTag("result", "success");
}
catch (Exception ex)
{
    activity?.SetTag("result", "failure");
    activity?.SetTag("error", ex.Message);
    throw;
}
finally
{
    stopwatch.Stop();
    activity?.SetTag("duration_ms", stopwatch.ElapsedMilliseconds);
}
```

## Sample Application Issues

### Console Sample Issues

#### Problem: OAuth Flow Not Completing
```
Error: Authorization code not provided
Error: Browser not opening automatically
```

**Solutions:**
1. **Manual Browser Opening**:
   ```
   Copy the authorization URL manually and paste into browser
   After authorization, copy the code from the callback URL
   ```

2. **Alternative Redirect URI**:
   ```json
   {
     "ProcoreAuth": {
       "RedirectUri": "urn:ietf:wg:oauth:2.0:oob"
     }
   }
   ```

### Web Sample Issues

#### Problem: Session Storage Not Working
```
Error: Token not persisted across requests
Error: Session state lost
```

**Solutions:**
1. **Session Configuration**:
   ```csharp
   services.AddSession(options =>
   {
       options.IdleTimeout = TimeSpan.FromMinutes(30);
       options.Cookie.IsEssential = true;
   });
   ```

2. **HTTPS Requirement**:
   ```csharp
   options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // For development
   ```

## Production Deployment Issues

### Token Security

#### Problem: Token Storage in Production
```
Error: Tokens exposed in logs
Error: Insecure token storage
```

**Solutions:**
1. **Use Secure Token Storage**:
   ```csharp
   // Use encrypted storage in production
   services.AddScoped<ITokenStorage, EncryptedTokenStorage>();
   ```

2. **Implement Token Rotation**:
   ```csharp
   services.Configure<ProcoreAuthOptions>(options =>
   {
       options.TokenRefreshMargin = TimeSpan.FromMinutes(5);
       options.AutomaticTokenRefresh = true;
   });
   ```

### Monitoring and Observability

#### Problem: Lack of Production Monitoring
```
Error: No visibility into API failures
```

**Solutions:**
1. **Add Application Insights**:
   ```csharp
   services.AddApplicationInsightsTelemetry();
   ```

2. **Custom Metrics**:
   ```csharp
   services.AddSingleton<IMetrics, CustomMetrics>();
   ```

## Performance Optimization

### Client Configuration

```csharp
services.AddHttpClient<ProcoreCoreClient>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    MaxConnectionsPerServer = 10
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());
```

### Connection Pooling

```csharp
services.Configure<HttpClientFactoryOptions>(options =>
{
    options.HandlerLifetime = TimeSpan.FromMinutes(5);
});
```

## Support Resources

### Documentation
- [Procore API Documentation](https://developers.procore.com/documentation)
- [SDK GitHub Repository](https://github.com/bskertchly/procore-sdk)
- [Integration Test Examples](./Procore.SDK.IntegrationTests.Live/)

### Community Support
- GitHub Issues for bug reports
- GitHub Discussions for questions
- Stack Overflow tag: `procore-sdk`

### Professional Support
Contact your Procore representative for enterprise support options.

---

**Note**: This troubleshooting guide covers common scenarios. For complex issues, enable debug logging and examine the detailed error messages and stack traces for more specific guidance.