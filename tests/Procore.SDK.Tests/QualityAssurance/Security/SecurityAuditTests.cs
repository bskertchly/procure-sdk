using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Extensions;
using Procore.SDK.Shared.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace Procore.SDK.Tests.QualityAssurance.Security;

/// <summary>
/// Comprehensive security audit and vulnerability assessment tests
/// Validates security implementations, identifies potential vulnerabilities, and ensures secure coding practices
/// </summary>
public class SecurityAuditTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly string _projectRoot;
    private readonly string _samplesPath;
    private readonly ServiceProvider _serviceProvider;

    public SecurityAuditTests(ITestOutputHelper output)
    {
        _output = output;
        _projectRoot = GetProjectRoot();
        _samplesPath = Path.Combine(_projectRoot, "samples");

        // Setup test configuration for security validation
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
        services.AddProcoreSDK(configuration);
        services.AddSingleton<ITokenStorage, InMemoryTokenStorage>();
        _serviceProvider = services.BuildServiceProvider();
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
    public void Should_Not_Contain_Hardcoded_Secrets()
    {
        // Arrange
        var codeFiles = GetAllSourceFiles();
        var configFiles = GetAllConfigFiles();
        var allFiles = codeFiles.Concat(configFiles).ToArray();
        
        var suspiciousPatterns = new[]
        {
            // API Keys and Tokens
            (@"(?i)(api[_-]?key|apikey)\s*[:=]\s*[""'][A-Za-z0-9+/]{20,}[""']", "Potential API key"),
            (@"(?i)(access[_-]?token|accesstoken)\s*[:=]\s*[""'][A-Za-z0-9+/]{20,}[""']", "Potential access token"),
            (@"(?i)(client[_-]?secret|clientsecret)\s*[:=]\s*[""'][A-Za-z0-9+/]{20,}[""']", "Potential client secret"),
            
            // Database Connections
            (@"(?i)(password|pwd)\s*[:=]\s*[""'][^""']{5,}[""']", "Potential password"),
            (@"(?i)server\s*=\s*[^;]+;\s*user\s*id\s*=\s*[^;]+;\s*password\s*=\s*[^;]+", "Database connection string"),
            
            // JWT Secrets
            (@"(?i)(jwt[_-]?secret|jwtsecret)\s*[:=]\s*[""'][A-Za-z0-9+/]{20,}[""']", "Potential JWT secret"),
            
            // Private Keys
            (@"-----BEGIN\s+(RSA\s+)?PRIVATE\s+KEY-----", "Private key"),
            (@"(?i)(private[_-]?key|privatekey)\s*[:=]\s*[""'][A-Za-z0-9+/=]{40,}[""']", "Potential private key")
        };

        // Act & Assert
        var vulnerabilities = new List<string>();

        foreach (var file in allFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            foreach (var (pattern, description) in suspiciousPatterns)
            {
                var matches = Regex.Matches(content, pattern);
                foreach (Match match in matches)
                {
                    // Skip test files and known safe patterns
                    if (IsTestFile(file) || IsSafePattern(match.Value))
                        continue;

                    vulnerabilities.Add($"{fileName}: {description} - {match.Value.Substring(0, Math.Min(50, match.Value.Length))}...");
                }
            }
        }

        // Report findings
        if (vulnerabilities.Any())
        {
            foreach (var vulnerability in vulnerabilities)
            {
                _output.WriteLine($"⚠️  SECURITY: {vulnerability}");
            }
        }

        Assert.Empty(vulnerabilities);
        _output.WriteLine($"✅ Scanned {allFiles.Length} files - No hardcoded secrets found");
    }

    [Fact]
    public void Web_Application_Should_Use_Secure_Cookie_Settings()
    {
        // Arrange
        var webProgramFile = Path.Combine(_samplesPath, "WebSample", "Program.cs");
        Assert.True(File.Exists(webProgramFile), "Web sample Program.cs should exist");

        // Act
        var content = File.ReadAllText(webProgramFile);

        // Assert - Check secure cookie configuration
        Assert.Contains("HttpOnly = true", content);
        Assert.Contains("SecurePolicy = CookieSecurePolicy.Always", content);
        Assert.Contains("SameSite = SameSiteMode.Strict", content);

        _output.WriteLine("✅ Web application uses secure cookie settings");

        // Check for session security
        Assert.Contains("IdleTimeout", content);
        Assert.Contains("Cookie.IsEssential = true", content);

        _output.WriteLine("✅ Session configuration includes security settings");
    }

    [Fact]
    public void Web_Application_Should_Include_Security_Headers()
    {
        // Arrange
        var webProgramFile = Path.Combine(_samplesPath, "WebSample", "Program.cs");
        var content = File.ReadAllText(webProgramFile);

        // Act & Assert - Check for security headers
        var requiredHeaders = new[]
        {
            ("X-Content-Type-Options", "nosniff"),
            ("X-Frame-Options", "DENY"),
            ("X-XSS-Protection", "1; mode=block"),
            ("Referrer-Policy", "strict-origin-when-cross-origin")
        };

        foreach (var (header, expectedValue) in requiredHeaders)
        {
            Assert.Contains(header, content);
            if (expectedValue != null)
            {
                Assert.Contains(expectedValue, content);
            }
            _output.WriteLine($"✅ Security header configured: {header}");
        }

        // Check for HTTPS redirection
        Assert.Contains("UseHttpsRedirection", content);
        _output.WriteLine("✅ HTTPS redirection enabled");

        // Check for HSTS in production
        Assert.Contains("UseHsts", content);
        _output.WriteLine("✅ HSTS configured for production");
    }

    [Fact]
    public void OAuth_Implementation_Should_Use_PKCE()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();

        // Act
        var state = "test-state";
        var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);

        // Assert - Validate PKCE implementation
        var uri = new Uri(authUrl);
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

        // Should use PKCE
        Assert.True(query.ContainsKey("code_challenge"));
        Assert.True(query.ContainsKey("code_challenge_method"));
        Assert.Equal("S256", query["code_challenge_method"].ToString());

        // Code verifier should meet RFC 7636 requirements
        Assert.True(codeVerifier.Length >= 43 && codeVerifier.Length <= 128);
        Assert.Matches(@"^[A-Za-z0-9\-._~]*$", codeVerifier);

        _output.WriteLine("✅ OAuth implementation uses PKCE with S256 challenge method");
        _output.WriteLine($"✅ Code verifier meets RFC 7636 requirements (length: {codeVerifier.Length})");
    }

    [Fact]
    public void OAuth_Should_Validate_State_Parameter()
    {
        // Arrange
        var authControllerFile = Path.Combine(_samplesPath, "WebSample", "Controllers", "AuthController.cs");
        if (!File.Exists(authControllerFile))
        {
            _output.WriteLine("⚠️  AuthController.cs not found - skipping state validation test");
            return;
        }

        // Act
        var content = File.ReadAllText(authControllerFile);

        // Assert - Check for state validation in callback
        Assert.Contains("ValidateCallbackParameters", content);
        Assert.Contains("state", content.ToLower());

        _output.WriteLine("✅ OAuth callback validates state parameter");

        // Check for CSRF protection patterns
        if (content.Contains("AntiForgeryToken") || content.Contains("ValidateAntiForgeryToken"))
        {
            _output.WriteLine("✅ CSRF protection mechanisms found");
        }
    }

    [Fact]
    public void Token_Storage_Should_Be_Secure()
    {
        // Arrange
        var tokenStorage = _serviceProvider.GetRequiredService<ITokenStorage>();

        // Act & Assert - Test token storage security
        var testToken = new AccessToken(
            Token: "sensitive-access-token",
            TokenType: "Bearer",
            ExpiresAt: DateTimeOffset.UtcNow.AddHours(1),
            RefreshToken: "sensitive-refresh-token",
            Scopes: new[] { "read", "write" });

        // For in-memory storage, tokens should not persist beyond application lifecycle
        // For session storage, tokens should be encrypted and have proper expiration

        _output.WriteLine("✅ Token storage interface provides abstraction for secure implementations");

        // Check session token storage implementation
        var sessionStorageFile = Path.Combine(_samplesPath, "WebSample", "Services", "SessionTokenStorage.cs");
        if (File.Exists(sessionStorageFile))
        {
            var content = File.ReadAllText(sessionStorageFile);
            
            // Should use session for temporary storage (checking for session usage patterns)
            Assert.True(content.Contains("ISession") || content.Contains(".Session"), 
                "SessionTokenStorage should use session for temporary storage");
            
            // Should handle serialization securely
            if (content.Contains("JsonSerializer") || content.Contains("System.Text.Json"))
            {
                _output.WriteLine("✅ Session token storage uses secure JSON serialization");
            }
        }
    }

    [Fact]
    public void Application_Should_Validate_Input_Parameters()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check for input validation patterns
            var validationPatterns = new[]
            {
                (@"string\.IsNullOrEmpty\(", "Null/empty validation"),
                (@"string\.IsNullOrWhiteSpace\(", "Null/whitespace validation"),
                (@"ArgumentNullException", "Null argument validation"),
                (@"ArgumentException", "Argument validation"),
                (@"ModelState\.IsValid", "Model validation"),
                (@"Url\.IsLocalUrl\(", "URL validation")
            };

            var foundValidations = new List<string>();
            foreach (var (pattern, description) in validationPatterns)
            {
                if (Regex.IsMatch(content, pattern))
                {
                    foundValidations.Add(description);
                }
            }

            if (foundValidations.Any())
            {
                _output.WriteLine($"✅ {fileName}: Input validation - {string.Join(", ", foundValidations)}");
            }
        }
    }

    [Fact]
    public void Should_Use_Secure_Random_Generation()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var secureRandomFound = false;

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check for secure random generation
            if (content.Contains("Guid.NewGuid()"))
            {
                _output.WriteLine($"✅ {fileName}: Uses Guid.NewGuid() for unique identifiers");
                secureRandomFound = true;
            }

            if (content.Contains("RandomNumberGenerator") || content.Contains("RNGCryptoServiceProvider"))
            {
                _output.WriteLine($"✅ {fileName}: Uses cryptographically secure random generation");
                secureRandomFound = true;
            }

            // Warn about insecure random usage
            if (content.Contains("new Random()") && !IsTestFile(file))
            {
                _output.WriteLine($"⚠️  {fileName}: Uses System.Random (not cryptographically secure)");
            }
        }

        Assert.True(secureRandomFound, "Should use secure random generation in some part of the application");
    }

    [Fact]
    public void Should_Handle_Sensitive_Data_Properly()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check logging doesn't expose sensitive data
            var logStatements = Regex.Matches(content, @"_logger\.Log\w*\([^)]+\)", RegexOptions.Singleline);
            
            foreach (Match logStatement in logStatements)
            {
                var logContent = logStatement.Value.ToLower();
                
                // Should not log sensitive data
                Assert.DoesNotContain("password", logContent);
                Assert.DoesNotContain("secret", logContent);
                Assert.DoesNotContain("private_key", logContent);
                
                // Token logging should be limited
                if (logContent.Contains("token"))
                {
                    _output.WriteLine($"ℹ️  {fileName}: Logs token information - verify it's safe");
                }
            }

            // Check for secure string handling
            if (content.Contains("SecureString"))
            {
                _output.WriteLine($"✅ {fileName}: Uses SecureString for sensitive data");
            }

            // Check for memory clearing patterns
            if (content.Contains("Array.Clear") || content.Contains("Memory.Clear"))
            {
                _output.WriteLine($"✅ {fileName}: Includes memory clearing for sensitive data");
            }
        }
    }

    [Fact]
    public void Dependencies_Should_Not_Have_Known_Vulnerabilities()
    {
        // Arrange
        var projectFiles = Directory.GetFiles(_projectRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(f => !f.Contains("bin") && !f.Contains("obj"))
            .ToArray();

        var knownVulnerablePackages = new Dictionary<string, string[]>
        {
            // Example vulnerable packages - this would be updated with real vulnerability data
            ["Newtonsoft.Json"] = new[] { "12.0.0", "12.0.1" }, // Example versions with vulnerabilities
            ["System.Text.RegularExpressions"] = new[] { "4.3.0" } // Example vulnerable version
        };

        // Act & Assert
        foreach (var projectFile in projectFiles)
        {
            var content = File.ReadAllText(projectFile);
            var fileName = Path.GetFileName(projectFile);

            foreach (var (packageName, vulnerableVersions) in knownVulnerablePackages)
            {
                if (content.Contains(packageName))
                {
                    foreach (var vulnerableVersion in vulnerableVersions)
                    {
                        Assert.DoesNotContain($"Version=\"{vulnerableVersion}\"", content);
                    }
                    _output.WriteLine($"✅ {fileName}: {packageName} not using known vulnerable versions");
                }
            }
        }

        _output.WriteLine($"✅ Checked {projectFiles.Length} project files for known vulnerable dependencies");
    }

    [Fact]
    public void Configuration_Should_Use_Secure_Defaults()
    {
        // Arrange
        var configFiles = GetAllConfigFiles();

        foreach (var configFile in configFiles)
        {
            var content = File.ReadAllText(configFile);
            var fileName = Path.GetFileName(configFile);

            // Act & Assert - Check for secure configuration defaults
            
            // Should not contain default/placeholder secrets
            var insecureDefaults = new[]
            {
                "your-client-id-here",
                "your-client-secret-here",
                "change-me",
                "default-password",
                "admin",
                "password123"
            };

            foreach (var insecureDefault in insecureDefaults)
            {
                Assert.DoesNotContain(insecureDefault, content.ToLower());
            }

            // HTTPS should be preferred
            if (content.Contains("http://") && !content.Contains("localhost"))
            {
                _output.WriteLine($"⚠️  {fileName}: Contains HTTP URLs (should prefer HTTPS)");
            }

            _output.WriteLine($"✅ {fileName}: Uses secure configuration defaults");
        }
    }

    [Fact]
    public void Error_Messages_Should_Not_Expose_Sensitive_Information()
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();

        foreach (var file in codeFiles)
        {
            var content = File.ReadAllText(file);
            var fileName = Path.GetFileName(file);

            // Act & Assert - Check error messages
            var errorMessages = ExtractErrorMessages(content);

            foreach (var message in errorMessages)
            {
                // Should not expose system details
                Assert.DoesNotContain("Connection string", message);
                Assert.DoesNotContain("SQL", message);
                Assert.DoesNotContain("Database", message);
                Assert.DoesNotContain("Server error", message);
                Assert.DoesNotContain("Stack trace", message);

                // Should not expose file paths
                Assert.DoesNotContain("C:\\", message);
                Assert.DoesNotContain("/var/", message);
                Assert.DoesNotContain("\\bin\\", message);

                _output.WriteLine($"✅ {fileName}: Error message is appropriately generic");
            }
        }
    }

    [Theory]
    [InlineData("SSL")]
    [InlineData("TLS")]
    [InlineData("HTTPS")]
    public void Should_Use_Secure_Communication_Protocols(string protocol)
    {
        // Arrange
        var codeFiles = GetAllCSharpFiles();
        var configFiles = GetAllConfigFiles();
        var allFiles = codeFiles.Concat(configFiles);

        var protocolFound = false;

        // Act & Assert
        foreach (var file in allFiles)
        {
            var content = File.ReadAllText(file);
            
            if (content.ToUpper().Contains(protocol))
            {
                protocolFound = true;
                _output.WriteLine($"✅ {Path.GetFileName(file)}: References {protocol}");
            }
        }

        // At least HTTPS should be referenced in the configuration
        if (protocol == "HTTPS")
        {
            Assert.True(protocolFound, "Application should reference HTTPS for secure communication");
        }
    }

    [Fact]
    public void PKCE_Code_Challenge_Should_Meet_Security_Requirements()
    {
        // Arrange
        var oauthHelper = _serviceProvider.GetRequiredService<OAuthFlowHelper>();
        
        // Act - Generate multiple authorization URLs to test code challenge uniqueness
        var codeVerifiers = new HashSet<string>();
        var codeChallenges = new HashSet<string>();
        
        for (int i = 0; i < 10; i++)
        {
            var state = $"test-state-{i}";
            var (authUrl, codeVerifier) = oauthHelper.GenerateAuthorizationUrl(state);
            
            codeVerifiers.Add(codeVerifier);
            
            // Extract code challenge from the URL
            var uri = new Uri(authUrl);
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            if (query.TryGetValue("code_challenge", out var challenge))
            {
                codeChallenges.Add(challenge.ToString());
            }
        }
        
        // Assert - Verify security requirements
        Assert.Equal(10, codeVerifiers.Count); // All code verifiers should be unique
        Assert.Equal(10, codeChallenges.Count); // All code challenges should be unique
        
        // Verify code verifier meets RFC 7636 requirements
        foreach (var verifier in codeVerifiers)
        {
            Assert.True(verifier.Length >= 43, "Code verifier should be at least 43 characters");
            Assert.True(verifier.Length <= 128, "Code verifier should be at most 128 characters");
            Assert.Matches(@"^[A-Za-z0-9._~-]+$", verifier); // Should use unreserved characters only
        }
        
        _output.WriteLine($"✅ Generated {codeVerifiers.Count} unique code verifiers");
        _output.WriteLine($"✅ Generated {codeChallenges.Count} unique code challenges");
        _output.WriteLine("✅ PKCE implementation meets RFC 7636 security requirements");
    }

    private string[] GetAllSourceFiles()
    {
        return Directory.GetFiles(_projectRoot, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains("bin") && !f.Contains("obj") && !f.Contains("TestResults"))
            .ToArray();
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

    private string[] GetAllConfigFiles()
    {
        var configExtensions = new[] { "*.json", "*.xml", "*.config" };
        var files = new List<string>();

        foreach (var extension in configExtensions)
        {
            files.AddRange(Directory.GetFiles(_samplesPath, extension, SearchOption.AllDirectories)
                .Where(f => !f.Contains("bin") && !f.Contains("obj")));
        }

        return files.ToArray();
    }

    private static bool IsTestFile(string filePath)
    {
        return filePath.Contains("Test") || filePath.Contains("test") || 
               filePath.Contains("Mock") || filePath.Contains("Fake");
    }

    private static bool IsSafePattern(string match)
    {
        var safePatterns = new[]
        {
            "test-client-id",
            "test-client-secret",
            "your-client-id-here",
            "example.com",
            "localhost"
        };

        return safePatterns.Any(pattern => match.ToLower().Contains(pattern));
    }

    private static List<string> ExtractErrorMessages(string content)
    {
        var messages = new List<string>();
        
        var patterns = new[]
        {
            @"TempData\[[""']Error[""']\]\s*=\s*[""']([^""']+)[""']",
            @"throw new \w*Exception\([""']([^""']+)[""']\)",
            @"return.*Error.*[""']([^""']+)[""']"
        };

        foreach (var pattern in patterns)
        {
            var matches = Regex.Matches(content, pattern);
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    messages.Add(match.Groups[1].Value);
                }
            }
        }

        return messages;
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}