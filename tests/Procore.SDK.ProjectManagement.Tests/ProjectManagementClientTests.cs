using Procore.SDK.ProjectManagement.Tests.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace Procore.SDK.ProjectManagement.Tests;

/// <summary>
/// Tests for the ProjectManagement client wrapper implementation.
/// These tests validate the actual implementation of the wrapper that provides
/// domain-specific convenience methods over the generated Kiota client.
/// </summary>
public class ProjectManagementClientTests : IDisposable
{
    private readonly IRequestAdapter _mockRequestAdapter;
    private readonly ITokenManager _mockTokenManager;
    private readonly ILogger<object> _mockLogger;
    private readonly object _mockGeneratedClient;
    private readonly ProjectManagementClient _sut;

    public ProjectManagementClientTests()
    {
        _mockRequestAdapter = Substitute.For<IRequestAdapter>();
        _mockTokenManager = Substitute.For<ITokenManager>();
        _mockLogger = new NullLogger<object>();
        _mockGeneratedClient = Substitute.For<object>();

        // Setup mock generated client with request adapter
        var mockGeneratedClient = _mockGeneratedClient;
        
        // Create system under test
        _sut = new ProjectManagementClient(_mockGeneratedClient, _mockTokenManager, _mockLogger);
    }

    #region Project Operations Tests

    [Fact]
    public async Task GetProjectsAsync_Should_Return_Projects_For_Company()
    {
        // Arrange
        var companyId = 123;
        var expectedProjects = new List<Project>
        {
            new() { Id = 1, Name = "Office Building", Status = ProjectStatus.Active, CompanyId = companyId },
            new() { Id = 2, Name = "Warehouse", Status = ProjectStatus.Planning, CompanyId = companyId }
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project[]>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedProjects.ToArray());

        // Act
        var result = await _sut.GetProjectsAsync(companyId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Office Building");
        result.First().Status.Should().Be(ProjectStatus.Active);
        result.Last().Name.Should().Be("Warehouse");
        result.Last().Status.Should().Be(ProjectStatus.Planning);
    }

    [Fact]
    public async Task GetProjectAsync_Should_Return_Single_Project()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;
        var expectedProject = new Project
        {
            Id = projectId,
            Name = "Test Project",
            Description = "Test project description",
            Status = ProjectStatus.Active,
            CompanyId = companyId,
            Budget = 1000000.00m
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedProject);

        // Act
        var result = await _sut.GetProjectAsync(companyId, projectId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(projectId);
        result.Name.Should().Be("Test Project");
        result.Status.Should().Be(ProjectStatus.Active);
        result.Budget.Should().Be(1000000.00m);
    }

    [Fact]
    public async Task CreateProjectAsync_Should_Create_New_Project()
    {
        // Arrange
        var companyId = 123;
        var request = new CreateProjectRequest
        {
            Name = "New Project",
            Description = "New project description",
            Budget = 500000.00m,
            ProjectType = "Commercial"
        };

        var expectedProject = new Project
        {
            Id = 789,
            Name = request.Name,
            Description = request.Description,
            Status = ProjectStatus.Planning,
            CompanyId = companyId,
            Budget = request.Budget
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedProject);

        // Act
        var result = await _sut.CreateProjectAsync(companyId, request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New Project");
        result.Description.Should().Be("New project description");
        result.Budget.Should().Be(500000.00m);
        result.Status.Should().Be(ProjectStatus.Planning);
    }

    [Fact]
    public async Task UpdateProjectAsync_Should_Update_Existing_Project()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;
        var request = new UpdateProjectRequest
        {
            Name = "Updated Project Name",
            Status = ProjectStatus.Active,
            Budget = 750000.00m
        };

        var expectedProject = new Project
        {
            Id = projectId,
            Name = request.Name.Value,
            Status = request.Status.Value,
            CompanyId = companyId,
            Budget = request.Budget
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedProject);

        // Act
        var result = await _sut.UpdateProjectAsync(companyId, projectId, request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Updated Project Name");
        result.Status.Should().Be(ProjectStatus.Active);
        result.Budget.Should().Be(750000.00m);
    }

    [Fact]
    public async Task DeleteProjectAsync_Should_Complete_Successfully()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<object>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<object?>(null));

        // Act & Assert - Should not throw
        await _sut.DeleteProjectAsync(companyId, projectId);

        // Verify the request was sent
        await _mockRequestAdapter.Received(1).SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<object>>(),
            Arg.Any<CancellationToken>());
    }

    #endregion

    #region Budget Operations Tests

    [Fact]
    public async Task GetBudgetLineItemsAsync_Should_Return_Budget_Items()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;
        var expectedItems = new List<BudgetLineItem>
        {
            new() { Id = 1, ProjectId = projectId, WbsCode = "01.01", Description = "Site Work", BudgetAmount = 100000.00m },
            new() { Id = 2, ProjectId = projectId, WbsCode = "02.01", Description = "Concrete", BudgetAmount = 200000.00m }
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BudgetLineItem[]>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedItems.ToArray());

        // Act
        var result = await _sut.GetBudgetLineItemsAsync(companyId, projectId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().WbsCode.Should().Be("01.01");
        result.First().BudgetAmount.Should().Be(100000.00m);
    }

    [Fact]
    public async Task CreateBudgetChangeAsync_Should_Create_Budget_Modification()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;
        var request = new CreateBudgetChangeRequest
        {
            Description = "Budget increase for material costs",
            Amount = 50000.00m,
            LineItems = new List<BudgetLineItemChange>
            {
                new() { LineItemId = 1, Amount = 25000.00m, Reason = "Steel price increase" },
                new() { LineItemId = 2, Amount = 25000.00m, Reason = "Concrete price increase" }
            }
        };

        var expectedChange = new BudgetChange
        {
            Id = 789,
            ProjectId = projectId,
            Description = request.Description,
            Amount = request.Amount,
            Status = BudgetChangeStatus.Pending
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BudgetChange>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedChange);

        // Act
        var result = await _sut.CreateBudgetChangeAsync(companyId, projectId, request);

        // Assert
        result.Should().NotBeNull();
        result.Description.Should().Be("Budget increase for material costs");
        result.Amount.Should().Be(50000.00m);
        result.Status.Should().Be(BudgetChangeStatus.Pending);
    }

    #endregion

    #region Contract Operations Tests

    [Fact]
    public async Task GetCommitmentContractsAsync_Should_Return_Contracts()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;
        var expectedContracts = new List<CommitmentContract>
        {
            new() { Id = 1, ProjectId = projectId, Title = "Electrical Contract", ContractAmount = 300000.00m, Status = ContractStatus.Executed },
            new() { Id = 2, ProjectId = projectId, Title = "Plumbing Contract", ContractAmount = 150000.00m, Status = ContractStatus.Pending }
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<CommitmentContract[]>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedContracts.ToArray());

        // Act
        var result = await _sut.GetCommitmentContractsAsync(companyId, projectId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Title.Should().Be("Electrical Contract");
        result.First().ContractAmount.Should().Be(300000.00m);
        result.First().Status.Should().Be(ContractStatus.Executed);
    }

    [Fact]
    public async Task CreateChangeOrderAsync_Should_Create_Change_Order()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;
        var request = new CreateChangeOrderRequest
        {
            Title = "Additional Electrical Work",
            Number = "CO-001",
            Amount = 25000.00m,
            Type = ChangeOrderType.Prime,
            Description = "Add emergency lighting"
        };

        var expectedChangeOrder = new ChangeOrder
        {
            Id = 101,
            ProjectId = projectId,
            Title = request.Title,
            Number = request.Number,
            Amount = request.Amount,
            Type = request.Type,
            Status = ChangeOrderStatus.Draft
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<ChangeOrder>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedChangeOrder);

        // Act
        var result = await _sut.CreateChangeOrderAsync(companyId, projectId, request);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Additional Electrical Work");
        result.Amount.Should().Be(25000.00m);
        result.Type.Should().Be(ChangeOrderType.Prime);
        result.Status.Should().Be(ChangeOrderStatus.Draft);
    }

    #endregion

    #region Convenience Methods Tests

    [Fact]
    public async Task GetActiveProjectsAsync_Should_Return_Only_Active_Projects()
    {
        // Arrange
        var companyId = 123;
        var allProjects = new List<Project>
        {
            new() { Id = 1, Name = "Active Project 1", Status = ProjectStatus.Active },
            new() { Id = 2, Name = "Completed Project", Status = ProjectStatus.Completed },
            new() { Id = 3, Name = "Active Project 2", Status = ProjectStatus.Active }
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project[]>>(),
            Arg.Any<CancellationToken>())
            .Returns(allProjects.Where(p => p.Status == ProjectStatus.Active).ToArray());

        // Act
        var result = await _sut.GetActiveProjectsAsync(companyId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.Status == ProjectStatus.Active);
    }

    [Fact]
    public async Task GetProjectByNameAsync_Should_Return_Project_With_Matching_Name()
    {
        // Arrange
        var companyId = 123;
        var projectName = "Office Building";
        var expectedProject = new Project
        {
            Id = 456,
            Name = projectName,
            Status = ProjectStatus.Active,
            CompanyId = companyId
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedProject);

        // Act
        var result = await _sut.GetProjectByNameAsync(companyId, projectName);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(projectName);
        result.Id.Should().Be(456);
    }

    [Fact]
    public async Task GetProjectBudgetTotalAsync_Should_Return_Total_Budget()
    {
        // Arrange
        var companyId = 123;
        var projectId = 456;
        var budgetItems = new List<BudgetLineItem>
        {
            new() { BudgetAmount = 100000.00m },
            new() { BudgetAmount = 200000.00m },
            new() { BudgetAmount = 150000.00m }
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<BudgetLineItem[]>>(),
            Arg.Any<CancellationToken>())
            .Returns(budgetItems.ToArray());

        // Act
        var result = await _sut.GetProjectBudgetTotalAsync(companyId, projectId);

        // Assert
        result.Should().Be(450000.00m);
    }

    #endregion

    #region Pagination Tests

    [Fact]
    public async Task GetProjectsPagedAsync_Should_Return_Paged_Results()
    {
        // Arrange
        var companyId = 123;
        var options = new PaginationOptions { Page = 1, PerPage = 10 };
        var projects = new List<Project>
        {
            new() { Id = 1, Name = "Project 1" },
            new() { Id = 2, Name = "Project 2" }
        };

        var expectedPagedResult = new PagedResult<Project>
        {
            Items = projects,
            TotalCount = 25,
            Page = 1,
            PerPage = 10,
            TotalPages = 3,
            HasNextPage = true,
            HasPreviousPage = false
        };

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<PagedResult<Project>>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedPagedResult);

        // Act
        var result = await _sut.GetProjectsPagedAsync(companyId, options);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(25);
        result.Page.Should().Be(1);
        result.PerPage.Should().Be(10);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task GetProjectAsync_Should_Throw_When_Project_Not_Found()
    {
        // Arrange
        var companyId = 123;
        var projectId = 999;

        var httpException = new HttpRequestException("Not Found");
        httpException.Data["StatusCode"] = HttpStatusCode.NotFound;

        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<CancellationToken>())
            .Throws(httpException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            () => _sut.GetProjectAsync(companyId, projectId));

        exception.Message.Should().Be("Not Found");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetProjectsAsync_Should_Throw_When_CompanyId_Invalid(int invalidCompanyId)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.GetProjectsAsync(invalidCompanyId));
    }

    #endregion

    #region Authentication Integration Tests

    [Fact]
    public async Task ProjectManagementClient_Should_Request_Token_Before_API_Calls()
    {
        // Arrange
        const string expectedToken = "valid_access_token";
        _mockTokenManager.GetAccessTokenAsync(Arg.Any<CancellationToken>())
            .Returns(expectedToken);

        var expectedProject = new Project { Id = 1, Name = "Test Project" };
        _mockRequestAdapter.SendAsync(
            Arg.Any<RequestInformation>(),
            Arg.Any<ParsableFactory<Project>>(),
            Arg.Any<CancellationToken>())
            .Returns(expectedProject);

        // Act
        await _sut.GetProjectAsync(123, 456);

        // Assert
        await _mockTokenManager.Received(1).GetAccessTokenAsync(Arg.Any<CancellationToken>());
    }

    #endregion

    #region Resource Management Tests

    [Fact]
    public void ProjectManagementClient_Should_Expose_RawClient()
    {
        // Act & Assert
        _sut.RawClient.Should().NotBeNull();
        _sut.RawClient.Should().Be(_mockGeneratedClient);
    }

    [Fact]
    public void ProjectManagementClient_Should_Implement_IDisposable()
    {
        // Act & Assert
        _sut.Should().BeAssignableTo<IDisposable>();
    }

    #endregion

    public void Dispose()
    {
        _sut?.Dispose();
    }
}

/// <summary>
/// Placeholder for the actual ProjectManagementClient implementation.
/// This will be replaced by the real implementation when Task 7 is completed.
/// </summary>
public class ProjectManagementClient : IProjectManagementClient
{
    private readonly object _generatedClient;
    private readonly ITokenManager _tokenManager;
    private readonly ILogger _logger;

    public ProjectManagementClient(object generatedClient, ITokenManager tokenManager, ILogger logger)
    {
        _generatedClient = generatedClient ?? throw new ArgumentNullException(nameof(generatedClient));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public object RawClient => _generatedClient;

    // All methods throw NotImplementedException for now
    // These will be implemented as part of Task 7
    public Task<IEnumerable<Project>> GetProjectsAsync(int companyId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<Project> GetProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<Project> CreateProjectAsync(int companyId, CreateProjectRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<Project> UpdateProjectAsync(int companyId, int projectId, UpdateProjectRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task DeleteProjectAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<BudgetLineItem>> GetBudgetLineItemsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<BudgetLineItem> GetBudgetLineItemAsync(int companyId, int projectId, int lineItemId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<BudgetChange> CreateBudgetChangeAsync(int companyId, int projectId, CreateBudgetChangeRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<BudgetChange>> GetBudgetChangesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<CommitmentContract>> GetCommitmentContractsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<CommitmentContract> GetCommitmentContractAsync(int companyId, int projectId, int contractId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<ChangeOrder> CreateChangeOrderAsync(int companyId, int projectId, CreateChangeOrderRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<ChangeOrder>> GetChangeOrdersAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<WorkflowInstance>> GetWorkflowInstancesAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<WorkflowInstance> GetWorkflowInstanceAsync(int companyId, int projectId, int instanceId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<WorkflowInstance> RestartWorkflowAsync(int companyId, int projectId, int instanceId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task TerminateWorkflowAsync(int companyId, int projectId, int instanceId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<Meeting>> GetMeetingsAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<Meeting> GetMeetingAsync(int companyId, int projectId, int meetingId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<Meeting> CreateMeetingAsync(int companyId, int projectId, CreateMeetingRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<Meeting> UpdateMeetingAsync(int companyId, int projectId, int meetingId, CreateMeetingRequest request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<Project>> GetActiveProjectsAsync(int companyId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<Project> GetProjectByNameAsync(int companyId, string projectName, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<decimal> GetProjectBudgetTotalAsync(int companyId, int projectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<IEnumerable<BudgetLineItem>> GetBudgetVariancesAsync(int companyId, int projectId, decimal thresholdAmount, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<PagedResult<Project>> GetProjectsPagedAsync(int companyId, PaginationOptions options, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<PagedResult<BudgetLineItem>> GetBudgetLineItemsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public Task<PagedResult<CommitmentContract>> GetCommitmentContractsPagedAsync(int companyId, int projectId, PaginationOptions options, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Will be implemented in Task 7");

    public void Dispose()
    {
        // Dispose implementation will be added in Task 7
    }
}