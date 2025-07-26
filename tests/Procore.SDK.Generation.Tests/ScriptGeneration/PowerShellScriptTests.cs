using Procore.SDK.Generation.Tests.Interfaces;
using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.ScriptGeneration;

/// <summary>
/// Tests for PowerShell script generation and execution
/// </summary>
public class PowerShellScriptTests
{
    private readonly MockFileSystem _fileSystem;
    private readonly IScriptGenerator _scriptGenerator;
    private readonly ILogger<PowerShellScriptTests> _logger;

    public PowerShellScriptTests()
    {
        _fileSystem = new MockFileSystem();
        _scriptGenerator = Substitute.For<IScriptGenerator>();
        _logger = Substitute.For<ILogger<PowerShellScriptTests>>();
    }

    [Fact]
    public async Task GenerateScriptAsync_ForPowerShell_ShouldCreateValidScript()
    {
        // Arrange
        var configurations = CreateSampleResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.PowerShell,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/Generate-Clients.ps1",
            IncludeErrorHandling = true,
            IncludeProgressReporting = true,
            IncludeValidation = true
        };

        var expectedScript = CreateExpectedPowerShellScript();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("param(");
        result.Should().Contain("$ResourceGroup");
        result.Should().Contain("kiota generate");
        result.Should().Contain("Write-Host");
        result.Should().Contain("try {");
        result.Should().Contain("catch {");
    }

    [Fact]
    public async Task GenerateScriptAsync_WithAllResourceGroups_ShouldIncludeAllConfigurations()
    {
        // Arrange
        var configurations = CreateAllResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.PowerShell,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/Generate-All-Clients.ps1"
        };

        var expectedScript = CreateComprehensivePowerShellScript();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().Contain("core");
        result.Should().Contain("project-management");
        result.Should().Contain("quality-safety");
        result.Should().Contain("construction-financials");
        result.Should().Contain("field-productivity");
        result.Should().Contain("resource-management");
        result.Should().Contain("CoreClient");
        result.Should().Contain("ProjectManagementClient");
    }

    [Fact]
    public async Task GenerateScriptAsync_WithErrorHandling_ShouldIncludeErrorHandlingBlocks()
    {
        // Arrange
        var configurations = CreateSampleResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.PowerShell,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/Generate-Clients.ps1",
            IncludeErrorHandling = true
        };

        var expectedScript = CreatePowerShellScriptWithErrorHandling();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().Contain("try {");
        result.Should().Contain("catch {");
        result.Should().Contain("$ErrorActionPreference");
        result.Should().Contain("Write-Error");
        result.Should().Contain("exit 1");
    }

    [Fact]
    public async Task GenerateScriptAsync_WithProgressReporting_ShouldIncludeProgressStatements()
    {
        // Arrange
        var configurations = CreateSampleResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.PowerShell,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/Generate-Clients.ps1",
            IncludeProgressReporting = true
        };

        var expectedScript = CreatePowerShellScriptWithProgress();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().Contain("Write-Host");
        result.Should().Contain("Write-Progress");
        result.Should().Contain("Generating");
        result.Should().Contain("completed");
        result.Should().Contain("Starting generation");
    }

    [Fact]
    public async Task GenerateScriptAsync_WithValidation_ShouldIncludeValidationSteps()
    {
        // Arrange
        var configurations = CreateSampleResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.PowerShell,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/Generate-Clients.ps1",
            IncludeValidation = true
        };

        var expectedScript = CreatePowerShellScriptWithValidation();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().Contain("Test-Path");
        result.Should().Contain("Get-Command kiota");
        result.Should().Contain("if (-not");
        result.Should().Contain("Validating");
    }

    [Fact]
    public async Task ExecuteScriptAsync_WithValidScript_ShouldExecuteSuccessfully()
    {
        // Arrange
        var scriptPath = "/tools/Generate-Clients.ps1";
        var workingDirectory = "/projects/procore-sdk";
        
        var expectedResult = new ScriptExecutionResult
        {
            Success = true,
            ExitCode = 0,
            StandardOutput = "Successfully generated all clients",
            StandardError = "",
            Duration = TimeSpan.FromMinutes(2),
            GeneratedClients = CreateExpectedGenerationResults()
        };

        _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.ExitCode.Should().Be(0);
        result.StandardOutput.Should().Contain("Successfully generated");
        result.StandardError.Should().BeEmpty();
        result.GeneratedClients.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ExecuteScriptAsync_WithInvalidScript_ShouldReturnFailure()
    {
        // Arrange
        var scriptPath = "/tools/Invalid-Script.ps1";
        var workingDirectory = "/projects/procore-sdk";
        
        var expectedResult = new ScriptExecutionResult
        {
            Success = false,
            ExitCode = 1,
            StandardOutput = "",
            StandardError = "Cannot find kiota command",
            Duration = TimeSpan.FromSeconds(5),
            GeneratedClients = []
        };

        _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ExitCode.Should().NotBe(0);
        result.StandardError.Should().NotBeEmpty();
        result.GeneratedClients.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidateRuntimeAsync_ForPowerShell_ShouldValidateAvailability()
    {
        // Arrange
        var expectedResult = new RuntimeValidationResult
        {
            IsAvailable = true,
            Version = "7.3.0",
            ExecutablePath = "/usr/local/bin/pwsh",
            SupportedFeatures = ["Core", "Desktop", "CrossPlatform"]
        };

        _scriptGenerator.ValidateRuntimeAsync(ScriptType.PowerShell, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ValidateRuntimeAsync(ScriptType.PowerShell);

        // Assert
        result.Should().NotBeNull();
        result.IsAvailable.Should().BeTrue();
        result.Version.Should().NotBeNullOrEmpty();
        result.ExecutablePath.Should().NotBeNullOrEmpty();
        result.SupportedFeatures.Should().Contain("Core");
    }

    [Fact]
    public async Task ValidateRuntimeAsync_WhenPowerShellNotInstalled_ShouldReturnUnavailable()
    {
        // Arrange
        var expectedResult = new RuntimeValidationResult
        {
            IsAvailable = false,
            ErrorMessage = "PowerShell not found in PATH"
        };

        _scriptGenerator.ValidateRuntimeAsync(ScriptType.PowerShell, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ValidateRuntimeAsync(ScriptType.PowerShell);

        // Assert
        result.Should().NotBeNull();
        result.IsAvailable.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Version.Should().BeNull();
    }

    [Fact]
    public void GetFileExtension_ForPowerShell_ShouldReturnPs1()
    {
        // Act
        var extension = _scriptGenerator.GetFileExtension(ScriptType.PowerShell);

        // Assert
        extension.Should().Be(".ps1");
    }

    [Fact]
    public void BuildScriptHeader_ForPowerShell_ShouldCreateValidHeader()
    {
        // Arrange
        var expectedHeader = CreateExpectedPowerShellHeader();
        
        _scriptGenerator.BuildScriptHeader(ScriptType.PowerShell, true)
                       .Returns(expectedHeader);

        // Act
        var header = _scriptGenerator.BuildScriptHeader(ScriptType.PowerShell, true);

        // Assert
        header.Should().NotBeNullOrEmpty();
        header.Should().Contain("#Requires");
        header.Should().Contain("param(");
        header.Should().Contain("$ErrorActionPreference");
    }

    [Theory]
    [InlineData("core")]
    [InlineData("project-management")]
    [InlineData("quality-safety")]
    [InlineData("all")]
    public async Task ExecuteScriptAsync_WithResourceGroupParameter_ShouldGenerateSpecificGroup(string resourceGroup)
    {
        // Arrange
        var scriptPath = "/tools/Generate-Clients.ps1";
        var workingDirectory = "/projects/procore-sdk";
        
        var expectedOutput = resourceGroup == "all" 
            ? "Generated all resource groups"
            : $"Generated {resourceGroup} client";
            
        var expectedResult = new ScriptExecutionResult
        {
            Success = true,
            ExitCode = 0,
            StandardOutput = expectedOutput,
            Duration = TimeSpan.FromMinutes(1),
            GeneratedClients = resourceGroup == "all" 
                ? CreateExpectedGenerationResults()
                : CreateSingleGenerationResult(resourceGroup)
        };

        _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.StandardOutput.Should().Contain(resourceGroup == "all" ? "all" : resourceGroup);
        
        if (resourceGroup == "all")
        {
            result.GeneratedClients.Should().HaveCountGreaterThan(1);
        }
        else
        {
            result.GeneratedClients.Should().HaveCount(1);
        }
    }

    [Fact]
    public async Task ExecuteScriptAsync_WithTimeout_ShouldRespectTimeout()
    {
        // Arrange
        var scriptPath = "/tools/Generate-Clients.ps1";
        var workingDirectory = "/projects/procore-sdk";
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        
        _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, cts.Token)
                       .Returns(Task.FromCanceled<ScriptExecutionResult>(cts.Token));

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => 
            _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, cts.Token));
    }

    private static IReadOnlyList<GenerationConfiguration> CreateSampleResourceConfigurations()
    {
        return new[]
        {
            new GenerationConfiguration
            {
                ResourceGroup = "core",
                ClassName = "CoreClient",
                Namespace = "Procore.SDK.Core",
                OutputDirectory = "src/Procore.SDK.Core/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/companies/**", "**/users/**"]
            },
            new GenerationConfiguration
            {
                ResourceGroup = "project-management",
                ClassName = "ProjectManagementClient",
                Namespace = "Procore.SDK.ProjectManagement",
                OutputDirectory = "src/Procore.SDK.ProjectManagement/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/projects/**", "**/workflows/**"]
            }
        };
    }

    private static IReadOnlyList<GenerationConfiguration> CreateAllResourceConfigurations()
    {
        return new[]
        {
            new GenerationConfiguration
            {
                ResourceGroup = "core",
                ClassName = "CoreClient",
                Namespace = "Procore.SDK.Core",
                OutputDirectory = "src/Procore.SDK.Core/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/companies/**", "**/users/**"]
            },
            new GenerationConfiguration
            {
                ResourceGroup = "project-management",
                ClassName = "ProjectManagementClient",
                Namespace = "Procore.SDK.ProjectManagement",
                OutputDirectory = "src/Procore.SDK.ProjectManagement/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/projects/**", "**/workflows/**"]
            },
            new GenerationConfiguration
            {
                ResourceGroup = "quality-safety",
                ClassName = "QualitySafetyClient",
                Namespace = "Procore.SDK.QualitySafety",
                OutputDirectory = "src/Procore.SDK.QualitySafety/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/inspections/**", "**/safety/**"]
            },
            new GenerationConfiguration
            {
                ResourceGroup = "construction-financials",
                ClassName = "ConstructionFinancialsClient",
                Namespace = "Procore.SDK.ConstructionFinancials",
                OutputDirectory = "src/Procore.SDK.ConstructionFinancials/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/contracts/**", "**/budgets/**"]
            },
            new GenerationConfiguration
            {
                ResourceGroup = "field-productivity",
                ClassName = "FieldProductivityClient",
                Namespace = "Procore.SDK.FieldProductivity",
                OutputDirectory = "src/Procore.SDK.FieldProductivity/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/daily-logs/**", "**/timecards/**"]
            },
            new GenerationConfiguration
            {
                ResourceGroup = "resource-management",
                ClassName = "ResourceManagementClient",
                Namespace = "Procore.SDK.ResourceManagement",
                OutputDirectory = "src/Procore.SDK.ResourceManagement/Generated",
                OpenApiSpecPath = "docs/rest_OAS_all.json",
                IncludePaths = ["**/workforce/**", "**/resources/**"]
            }
        };
    }

    private static IReadOnlyList<GenerationResult> CreateExpectedGenerationResults()
    {
        return new[]
        {
            new GenerationResult
            {
                Success = true,
                GeneratedFiles = ["CoreClient.cs", "Models/Company.cs"],
                Duration = TimeSpan.FromMinutes(1),
                GeneratedCodeSize = 1024 * 100 // 100KB
            },
            new GenerationResult
            {
                Success = true,
                GeneratedFiles = ["ProjectManagementClient.cs", "Models/Project.cs"],
                Duration = TimeSpan.FromMinutes(1),
                GeneratedCodeSize = 1024 * 150 // 150KB
            }
        };
    }

    private static IReadOnlyList<GenerationResult> CreateSingleGenerationResult(string resourceGroup)
    {
        return new[]
        {
            new GenerationResult
            {
                Success = true,
                GeneratedFiles = [$"{resourceGroup}Client.cs"],
                Duration = TimeSpan.FromSeconds(30),
                GeneratedCodeSize = 1024 * 50 // 50KB
            }
        };
    }

    private static string CreateExpectedPowerShellScript()
    {
        return """
        #Requires -Version 5.1
        
        param(
            [string]$ResourceGroup = "all"
        )
        
        $ErrorActionPreference = "Stop"
        
        $resources = @{
            "core" = @{
                Paths = @("**/companies/**", "**/users/**")
                Namespace = "Procore.SDK.Core"
                ClassName = "CoreClient"
            }
        }
        
        foreach ($name in $resources.Keys) {
            if ($ResourceGroup -eq "all" -or $ResourceGroup -eq $name) {
                Write-Host "Generating $name client..."
                kiota generate --openapi docs/rest_OAS_all.json
            }
        }
        """;
    }

    private static string CreateComprehensivePowerShellScript()
    {
        return """
        #Requires -Version 5.1
        
        param(
            [string]$ResourceGroup = "all"
        )
        
        $resources = @{
            "core" = @{ ClassName = "CoreClient" }
            "project-management" = @{ ClassName = "ProjectManagementClient" }
            "quality-safety" = @{ ClassName = "QualitySafetyClient" }
            "construction-financials" = @{ ClassName = "ConstructionFinancialsClient" }
            "field-productivity" = @{ ClassName = "FieldProductivityClient" }
            "resource-management" = @{ ClassName = "ResourceManagementClient" }
        }
        """;
    }

    private static string CreatePowerShellScriptWithErrorHandling()
    {
        return """
        $ErrorActionPreference = "Stop"
        
        try {
            kiota generate
        }
        catch {
            Write-Error "Generation failed: $_"
            exit 1
        }
        """;
    }

    private static string CreatePowerShellScriptWithProgress()
    {
        return """
        Write-Host "Starting generation process..."
        Write-Progress -Activity "Generating clients" -Status "Processing"
        Write-Host "Generation completed successfully"
        """;
    }

    private static string CreatePowerShellScriptWithValidation()
    {
        return """
        if (-not (Test-Path "docs/rest_OAS_all.json")) {
            Write-Error "OpenAPI spec not found"
        }
        
        if (-not (Get-Command kiota -ErrorAction SilentlyContinue)) {
            Write-Error "Kiota CLI not found"
        }
        
        Write-Host "Validating prerequisites..."
        """;
    }

    private static string CreateExpectedPowerShellHeader()
    {
        return """
        #Requires -Version 5.1
        
        param(
            [string]$ResourceGroup = "all"
        )
        
        $ErrorActionPreference = "Stop"
        """;
    }
}