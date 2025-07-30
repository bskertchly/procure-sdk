using Microsoft.Extensions.DependencyInjection;
using Procore.SDK.Shared.Authentication;

// Test program to validate package installation and basic functionality
namespace PackageValidation;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Testing Procore.SDK.Shared package installation...");
        
        // Test that ProcoreAuthOptions can be instantiated
        var authOptions = new ProcoreAuthOptions
        {
            ClientId = "test-client-id",
            RedirectUri = "https://localhost:3000/callback",
            Scopes = new[] { "read", "write" }
        };
        
        Console.WriteLine($"✅ ProcoreAuthOptions created with ClientId: {authOptions.ClientId}");
        
        // Test token storage interfaces
        var tokenStorage = new InMemoryTokenStorage();
        Console.WriteLine("✅ InMemoryTokenStorage instantiated successfully");
        
        // Test file token storage
        var fileTokenStorage = new FileTokenStorage("test-tokens.json");
        Console.WriteLine("✅ FileTokenStorage instantiated successfully");
        
        Console.WriteLine("✅ All package validation tests passed!");
        Console.WriteLine($"✅ Running on .NET {Environment.Version}");
        
        await Task.CompletedTask;
    }
}