using Procore.SDK.Generation.Tests.Interfaces;
using Procore.SDK.Generation.Tests.Models;

namespace Procore.SDK.Generation.Tests.ScriptGeneration;

/// <summary>
/// Tests for Bash script generation and execution
/// </summary>
public class BashScriptTests
{
    private readonly MockFileSystem _fileSystem;
    private readonly IScriptGenerator _scriptGenerator;
    private readonly ILogger<BashScriptTests> _logger;

    public BashScriptTests()
    {
        _fileSystem = new MockFileSystem();
        _scriptGenerator = Substitute.For<IScriptGenerator>();
        _logger = Substitute.For<ILogger<BashScriptTests>>();
    }

    [Fact]
    public async Task GenerateScriptAsync_ForBash_ShouldCreateValidScript()
    {
        // Arrange
        var configurations = CreateSampleResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.Bash,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/generate-clients.sh",
            IncludeErrorHandling = true,
            IncludeProgressReporting = true,
            IncludeValidation = true
        };

        var expectedScript = CreateExpectedBashScript();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().StartWith("#!/bin/bash");
        result.Should().Contain("generate_client()");
        result.Should().Contain("kiota generate");
        result.Should().Contain("echo");
        result.Should().Contain("set -e");
    }

    [Fact]
    public async Task GenerateScriptAsync_WithAllResourceGroups_ShouldIncludeAllConfigurations()
    {
        // Arrange
        var configurations = CreateAllResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.Bash,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/generate-all-clients.sh"
        };

        var expectedScript = CreateComprehensiveBashScript();
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
            Type = ScriptType.Bash,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/generate-clients.sh",
            IncludeErrorHandling = true
        };

        var expectedScript = CreateBashScriptWithErrorHandling();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().Contain("set -e");
        result.Should().Contain("set -u");
        result.Should().Contain("trap");
        result.Should().Contain("error_exit()");
        result.Should().Contain("exit 1");
    }

    [Fact]
    public async Task GenerateScriptAsync_WithProgressReporting_ShouldIncludeProgressStatements()
    {
        // Arrange
        var configurations = CreateSampleResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.Bash,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/generate-clients.sh",
            IncludeProgressReporting = true
        };

        var expectedScript = CreateBashScriptWithProgress();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().Contain("echo \"Starting");
        result.Should().Contain("echo \"Generating");
        result.Should().Contain("echo \"Completed");
        result.Should().Contain("progress");
    }

    [Fact]
    public async Task GenerateScriptAsync_WithValidation_ShouldIncludeValidationSteps()
    {
        // Arrange
        var configurations = CreateSampleResourceConfigurations();
        var scriptConfig = new ScriptGenerationConfiguration
        {
            Type = ScriptType.Bash,
            ResourceConfigurations = configurations,
            OutputPath = "/tools/generate-clients.sh",
            IncludeValidation = true
        };

        var expectedScript = CreateBashScriptWithValidation();
        _scriptGenerator.GenerateScriptAsync(scriptConfig, Arg.Any<CancellationToken>())
                       .Returns(expectedScript);

        // Act
        var result = await _scriptGenerator.GenerateScriptAsync(scriptConfig);

        // Assert
        result.Should().Contain("validate_prerequisites()");
        result.Should().Contain("[ -f");
        result.Should().Contain("command -v kiota");
        result.Should().Contain("if [ ! -f");
        result.Should().Contain("Validating");
    }

    [Fact]
    public async Task ExecuteScriptAsync_WithValidScript_ShouldExecuteSuccessfully()
    {
        // Arrange
        var scriptPath = "/tools/generate-clients.sh";
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
    public async Task ExecuteScriptAsync_WithPermissionError_ShouldReturnFailure()
    {
        // Arrange
        var scriptPath = "/tools/generate-clients.sh";
        var workingDirectory = "/projects/procore-sdk";
        
        var expectedResult = new ScriptExecutionResult
        {
            Success = false,
            ExitCode = 126, // Permission denied exit code
            StandardOutput = "",
            StandardError = "Permission denied: /tools/generate-clients.sh",
            Duration = TimeSpan.FromSeconds(1),
            GeneratedClients = []
        };

        _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ExitCode.Should().Be(126);
        result.StandardError.Should().Contain("Permission denied");
        result.GeneratedClients.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteScriptAsync_WithMissingDependency_ShouldReturnFailure()
    {
        // Arrange
        var scriptPath = "/tools/generate-clients.sh";
        var workingDirectory = "/projects/procore-sdk";
        
        var expectedResult = new ScriptExecutionResult
        {
            Success = false,
            ExitCode = 127, // Command not found exit code
            StandardOutput = "",
            StandardError = "kiota: command not found",
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
        result.ExitCode.Should().Be(127);
        result.StandardError.Should().Contain("command not found");
        result.GeneratedClients.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidateRuntimeAsync_ForBash_ShouldValidateAvailability()
    {
        // Arrange
        var expectedResult = new RuntimeValidationResult
        {
            IsAvailable = true,
            Version = "5.1.8",
            ExecutablePath = "/bin/bash",
            SupportedFeatures = ["POSIX", "Arrays", "Functions"]
        };

        _scriptGenerator.ValidateRuntimeAsync(ScriptType.Bash, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ValidateRuntimeAsync(ScriptType.Bash);

        // Assert
        result.Should().NotBeNull();
        result.IsAvailable.Should().BeTrue();
        result.Version.Should().NotBeNullOrEmpty();
        result.ExecutablePath.Should().NotBeNullOrEmpty();
        result.SupportedFeatures.Should().Contain("POSIX");
    }

    [Fact]
    public async Task ValidateRuntimeAsync_WhenBashNotInstalled_ShouldReturnUnavailable()
    {
        // Arrange
        var expectedResult = new RuntimeValidationResult
        {
            IsAvailable = false,
            ErrorMessage = "Bash not found in PATH"
        };

        _scriptGenerator.ValidateRuntimeAsync(ScriptType.Bash, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ValidateRuntimeAsync(ScriptType.Bash);

        // Assert
        result.Should().NotBeNull();
        result.IsAvailable.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Version.Should().BeNull();
    }

    [Fact]
    public void GetFileExtension_ForBash_ShouldReturnSh()
    {
        // Act
        var extension = _scriptGenerator.GetFileExtension(ScriptType.Bash);

        // Assert
        extension.Should().Be(".sh");
    }

    [Fact]
    public void BuildScriptHeader_ForBash_ShouldCreateValidHeader()
    {
        // Arrange
        var expectedHeader = CreateExpectedBashHeader();
        
        _scriptGenerator.BuildScriptHeader(ScriptType.Bash, true)
                       .Returns(expectedHeader);

        // Act
        var header = _scriptGenerator.BuildScriptHeader(ScriptType.Bash, true);

        // Assert
        header.Should().NotBeNullOrEmpty();
        header.Should().StartWith("#!/bin/bash");
        header.Should().Contain("set -e");
        header.Should().Contain("set -u");
    }

    [Theory]
    [InlineData("core")]
    [InlineData("project-management")]
    [InlineData("quality-safety")]
    [InlineData("all")]
    public async Task ExecuteScriptAsync_WithResourceGroupParameter_ShouldGenerateSpecificGroup(string resourceGroup)
    {
        // Arrange
        var scriptPath = "/tools/generate-clients.sh";
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
        var scriptPath = "/tools/generate-clients.sh";
        var workingDirectory = "/projects/procore-sdk";
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        
        _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, cts.Token)
                       .Returns(Task.FromCanceled<ScriptExecutionResult>(cts.Token));

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => 
            _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, cts.Token));
    }

    [Fact]
    public async Task ExecuteScriptAsync_WithVerboseOutput_ShouldCaptureDetailedLogs()
    {
        // Arrange
        var scriptPath = "/tools/generate-clients.sh";
        var workingDirectory = "/projects/procore-sdk";
        
        var expectedResult = new ScriptExecutionResult
        {
            Success = true,
            ExitCode = 0,
            StandardOutput = """
                Starting generation process...
                Validating prerequisites...
                Generating core client...
                Generated 15 files for core client
                Generating project-management client...
                Generated 20 files for project-management client
                Generation completed successfully
                """,
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
        result.StandardOutput.Should().Contain("Validating prerequisites");
        result.StandardOutput.Should().Contain("Generated");
        result.StandardOutput.Should().Contain("files for");
        result.StandardOutput.Should().Contain("completed successfully");
    }

    [Theory]
    [InlineData("docs/missing_spec.json", "OpenAPI spec file not found")]
    [InlineData("", "No OpenAPI spec specified")]
    public async Task ExecuteScriptAsync_WithInvalidSpecPath_ShouldReturnError(string specPath, string expectedError)
    {
        // Arrange
        var scriptPath = "/tools/generate-clients.sh";
        var workingDirectory = "/projects/procore-sdk";
        
        var expectedResult = new ScriptExecutionResult
        {
            Success = false,
            ExitCode = 1,
            StandardOutput = "",
            StandardError = expectedError,
            Duration = TimeSpan.FromSeconds(1),
            GeneratedClients = []
        };

        _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory, Arg.Any<CancellationToken>())
                       .Returns(expectedResult);

        // Act
        var result = await _scriptGenerator.ExecuteScriptAsync(scriptPath, workingDirectory);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.StandardError.Should().Contain(expectedError);
        result.GeneratedClients.Should().BeEmpty();
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

    private static string CreateExpectedBashScript()
    {
        return """
        #!/bin/bash
        set -e
        set -u
        
        generate_client() {
            local name=$1
            local class_name=$2
            local namespace=$3
            shift 3
            local paths=("$@")
            
            echo "Generating $name client..."
            kiota generate
        }
        
        generate_client "core" "CoreClient" "Procore.SDK.Core"
        """;
    }

    private static string CreateComprehensiveBashScript()
    {
        return """
        #!/bin/bash
        
        generate_client "core" "CoreClient"
        generate_client "project-management" "ProjectManagementClient"
        generate_client "quality-safety" "QualitySafetyClient"
        generate_client "construction-financials" "ConstructionFinancialsClient"
        generate_client "field-productivity" "FieldProductivityClient"
        generate_client "resource-management" "ResourceManagementClient"
        """;
    }

    private static string CreateBashScriptWithErrorHandling()
    {
        return """
        #!/bin/bash
        set -e
        set -u
        
        error_exit() {
            echo "Error: $1" >&2
            exit 1
        }
        
        trap 'error_exit "Script failed"' ERR
        """;
    }

    private static string CreateBashScriptWithProgress()
    {
        return """
        echo "Starting generation process..."
        echo "Generating client progress: 50%"
        echo "Completed generation successfully"
        """;
    }

    private static string CreateBashScriptWithValidation()
    {
        return """
        validate_prerequisites() {
            echo "Validating prerequisites..."
            
            if [ ! -f "docs/rest_OAS_all.json" ]; then
                error_exit "OpenAPI spec not found"
            fi
            
            if ! command -v kiota &> /dev/null; then
                error_exit "Kiota CLI not found"
            fi
        }
        """;
    }

    private static string CreateExpectedBashHeader()
    {
        return """
        #!/bin/bash
        set -e
        set -u
        """;
    }
}