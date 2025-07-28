namespace Procore.SDK.IntegrationTests.Live.Clients;

/// <summary>
/// Integration tests for QualitySafety Client operations against live Procore sandbox
/// Tests observations, inspections, and safety incidents with ~90% API coverage
/// </summary>
public class QualitySafetyClientIntegrationTests : IntegrationTestBase
{
    private Project? _testProject;

    public QualitySafetyClientIntegrationTests(LiveSandboxFixture fixture, ITestOutputHelper output) 
        : base(fixture, output) { }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        
        // Create a test project for QualitySafety operations
        _testProject = await CreateTestProjectAsync();
        Output.WriteLine($"Using test project: {_testProject.Id} - {_testProject.Name}");
    }

    #region Observation Operations (High Priority - Core Functionality)

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Observations")]
    [Trait("Priority", "High")]
    public async Task CreateObservation_Should_Create_Valid_Observation()
    {
        // Arrange
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id);

        // Act
        var observation = await ExecuteWithTrackingAsync("CreateObservation", async () =>
        {
            return await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);
        });

        // Assert
        ValidateApiResponse(observation, "CreateObservation");
        observation.Id.Should().BeGreaterThan(0, "Created observation should have valid ID");
        observation.Title.Should().Be(observationRequest.Title, "Title should match request");
        observation.Description.Should().Be(observationRequest.Description, "Description should match request");
        observation.ObservationType.Should().Be(observationRequest.ObservationType, "Type should match request");
        observation.Priority.Should().Be(observationRequest.Priority, "Priority should match request");
        
        Output.WriteLine($"Created observation: {observation.Id} - {observation.Title}");
        
        // Store for cleanup
        await CreateTestDataAsync($"observation_{observation.Id}", async () => observation);
        
        ValidatePerformance("CreateObservation", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Observations")]
    [Trait("Priority", "High")]
    public async Task GetObservations_Should_Return_Project_Observations()
    {
        // Arrange - Create test observation first
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id);
        var createdObservation = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);

        // Act
        var observations = await ExecuteWithTrackingAsync("GetObservations", async () =>
        {
            return await QualitySafetyClient.GetObservationsAsync(TestCompanyId, _testProject.Id);
        });

        // Assert
        ValidateApiResponse(observations, "GetObservations");
        observations.Should().NotBeEmpty("Should return at least the created observation");
        observations.Should().Contain(o => o.Id == createdObservation.Id, "Should contain the created observation");
        
        var retrievedObservation = observations.First(o => o.Id == createdObservation.Id);
        retrievedObservation.Title.Should().Be(createdObservation.Title, "Retrieved observation should match created");
        
        Output.WriteLine($"Retrieved {observations.Count()} observations for project {_testProject.Id}");
        
        ValidatePerformance("GetObservations", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Observations")]
    [Trait("Priority", "High")]
    public async Task GetObservation_Should_Return_Specific_Observation_Details()
    {
        // Arrange - Create test observation
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id);
        var createdObservation = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);

        // Act
        var observation = await ExecuteWithTrackingAsync("GetObservation", async () =>
        {
            return await QualitySafetyClient.GetObservationAsync(TestCompanyId, createdObservation.Id);
        });

        // Assert
        ValidateApiResponse(observation, "GetObservation");
        observation.Id.Should().Be(createdObservation.Id, "Should return requested observation");
        observation.Title.Should().Be(createdObservation.Title, "Title should match");
        observation.Description.Should().Be(createdObservation.Description, "Description should match");
        observation.Priority.Should().Be(createdObservation.Priority, "Priority should match");
        
        Output.WriteLine($"Retrieved observation details: {observation.Id} - {observation.Title}");
        
        ValidatePerformance("GetObservation", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Observations")]
    [Trait("Priority", "High")]
    public async Task UpdateObservation_Should_Modify_Observation_Successfully()
    {
        // Arrange - Create test observation
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id);
        var createdObservation = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);

        var updateRequest = new UpdateObservationRequest
        {
            Title = "Updated " + createdObservation.Title,
            Description = "Updated " + createdObservation.Description,
            Priority = "High",
            Status = "In Progress"
        };

        // Act
        var updatedObservation = await ExecuteWithTrackingAsync("UpdateObservation", async () =>
        {
            return await QualitySafetyClient.UpdateObservationAsync(TestCompanyId, createdObservation.Id, updateRequest);
        });

        // Assert
        ValidateApiResponse(updatedObservation, "UpdateObservation");
        updatedObservation.Id.Should().Be(createdObservation.Id, "ID should remain the same");
        updatedObservation.Title.Should().Be(updateRequest.Title, "Title should be updated");
        updatedObservation.Description.Should().Be(updateRequest.Description, "Description should be updated");
        updatedObservation.Priority.Should().Be(updateRequest.Priority, "Priority should be updated");
        
        Output.WriteLine($"Updated observation: {updatedObservation.Id} - {updatedObservation.Title}");
        
        ValidatePerformance("UpdateObservation", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Observations")]
    [Trait("Priority", "Medium")]
    public async Task GetObservations_With_Filters_Should_Return_Filtered_Results()
    {
        // Arrange - Create observations with different priorities
        var highPriorityRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id);
        highPriorityRequest.Priority = "High";
        var highPriorityObs = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, highPriorityRequest);

        var lowPriorityRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject.Id);
        lowPriorityRequest.Priority = "Low";
        var lowPriorityObs = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, lowPriorityRequest);

        // Act - Filter by high priority
        var filteredObservations = await ExecuteWithTrackingAsync("GetObservations_Filtered", async () =>
        {
            return await QualitySafetyClient.GetObservationsAsync(TestCompanyId, _testProject.Id, priority: "High");
        });

        // Assert
        ValidateApiResponse(filteredObservations, "GetObservations_Filtered");
        filteredObservations.Should().Contain(o => o.Id == highPriorityObs.Id, "Should contain high priority observation");
        filteredObservations.Should().OnlyContain(o => o.Priority == "High", "Should only contain high priority observations");
        
        Output.WriteLine($"Filtered observations: {filteredObservations.Count()} high priority observations");
        
        ValidatePerformance("GetObservations_Filtered", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Inspection Operations (Medium Priority)

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Inspections")]
    [Trait("Priority", "Medium")]
    public async Task CreateInspection_Should_Create_Valid_Inspection()
    {
        // Arrange
        var inspectionRequest = TestDataBuilder<CreateInspectionRequest>.CreateRealisticInspection(_testProject!.Id);

        // Act
        var inspection = await ExecuteWithTrackingAsync("CreateInspection", async () =>
        {
            return await QualitySafetyClient.CreateInspectionAsync(TestCompanyId, inspectionRequest);
        });

        // Assert
        ValidateApiResponse(inspection, "CreateInspection");
        inspection.Id.Should().BeGreaterThan(0, "Created inspection should have valid ID");
        inspection.Title.Should().Be(inspectionRequest.Title, "Title should match request");
        inspection.InspectionType.Should().Be(inspectionRequest.InspectionType, "Type should match request");
        inspection.Status.Should().NotBeNullOrEmpty("Status should be set");
        
        Output.WriteLine($"Created inspection: {inspection.Id} - {inspection.Title}");
        
        await CreateTestDataAsync($"inspection_{inspection.Id}", async () => inspection);
        
        ValidatePerformance("CreateInspection", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Inspections")]
    [Trait("Priority", "Medium")]
    public async Task GetInspections_Should_Return_Project_Inspections()
    {
        // Arrange - Create test inspection
        var inspectionRequest = TestDataBuilder<CreateInspectionRequest>.CreateRealisticInspection(_testProject!.Id);
        var createdInspection = await QualitySafetyClient.CreateInspectionAsync(TestCompanyId, inspectionRequest);

        // Act
        var inspections = await ExecuteWithTrackingAsync("GetInspections", async () =>
        {
            return await QualitySafetyClient.GetInspectionsAsync(TestCompanyId, _testProject.Id);
        });

        // Assert
        ValidateApiResponse(inspections, "GetInspections");
        inspections.Should().Contain(i => i.Id == createdInspection.Id, "Should contain the created inspection");
        
        Output.WriteLine($"Retrieved {inspections.Count()} inspections for project {_testProject.Id}");
        
        ValidatePerformance("GetInspections", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Inspections")]
    [Trait("Priority", "Medium")]
    public async Task CompleteInspection_Should_Change_Status_To_Completed()
    {
        // Arrange - Create test inspection
        var inspectionRequest = TestDataBuilder<CreateInspectionRequest>.CreateRealisticInspection(_testProject!.Id);
        var createdInspection = await QualitySafetyClient.CreateInspectionAsync(TestCompanyId, inspectionRequest);

        // Act
        var completedInspection = await ExecuteWithTrackingAsync("CompleteInspection", async () =>
        {
            return await QualitySafetyClient.CompleteInspectionAsync(TestCompanyId, createdInspection.Id);
        });

        // Assert
        ValidateApiResponse(completedInspection, "CompleteInspection");
        completedInspection.Id.Should().Be(createdInspection.Id, "ID should remain the same");
        completedInspection.Status.Should().Be("Completed", "Status should be completed");
        completedInspection.CompletedAt.Should().NotBeNull("Completed date should be set");
        
        Output.WriteLine($"Completed inspection: {completedInspection.Id} at {completedInspection.CompletedAt}");
        
        ValidatePerformance("CompleteInspection", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Safety Incident Operations (High Priority - Critical Functionality)

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "SafetyIncidents")]
    [Trait("Priority", "High")]
    public async Task CreateSafetyIncident_Should_Create_Valid_Incident()
    {
        // Arrange
        var incidentRequest = TestDataBuilder<CreateSafetyIncidentRequest>.CreateRealisticSafetyIncident(_testProject!.Id);

        // Act
        var incident = await ExecuteWithTrackingAsync("CreateSafetyIncident", async () =>
        {
            return await QualitySafetyClient.CreateSafetyIncidentAsync(TestCompanyId, incidentRequest);
        });

        // Assert
        ValidateApiResponse(incident, "CreateSafetyIncident");
        incident.Id.Should().BeGreaterThan(0, "Created incident should have valid ID");
        incident.IncidentType.Should().Be(incidentRequest.IncidentType, "Type should match request");
        incident.Description.Should().Be(incidentRequest.Description, "Description should match request");
        incident.InjuredPersonName.Should().Be(incidentRequest.InjuredPersonName, "Injured person should match request");
        
        Output.WriteLine($"Created safety incident: {incident.Id} - {incident.IncidentType}");
        
        await CreateTestDataAsync($"incident_{incident.Id}", async () => incident);
        
        ValidatePerformance("CreateSafetyIncident", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "SafetyIncidents")]
    [Trait("Priority", "High")]
    public async Task GetSafetyIncidents_Should_Return_Project_Incidents()
    {
        // Arrange - Create test incident
        var incidentRequest = TestDataBuilder<CreateSafetyIncidentRequest>.CreateRealisticSafetyIncident(_testProject!.Id);
        var createdIncident = await QualitySafetyClient.CreateSafetyIncidentAsync(TestCompanyId, incidentRequest);

        // Act
        var incidents = await ExecuteWithTrackingAsync("GetSafetyIncidents", async () =>
        {
            return await QualitySafetyClient.GetSafetyIncidentsAsync(TestCompanyId, _testProject.Id);
        });

        // Assert
        ValidateApiResponse(incidents, "GetSafetyIncidents");
        incidents.Should().Contain(i => i.Id == createdIncident.Id, "Should contain the created incident");
        
        Output.WriteLine($"Retrieved {incidents.Count()} safety incidents for project {_testProject.Id}");
        
        ValidatePerformance("GetSafetyIncidents", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "SafetyIncidents")]
    [Trait("Priority", "Medium")]
    public async Task GetSafetyIncidents_By_Type_Should_Filter_Correctly()
    {
        // Arrange - Create incidents of different types
        var nearMissRequest = TestDataBuilder<CreateSafetyIncidentRequest>.CreateRealisticSafetyIncident(_testProject!.Id);
        nearMissRequest.IncidentType = "Near Miss";
        var nearMissIncident = await QualitySafetyClient.CreateSafetyIncidentAsync(TestCompanyId, nearMissRequest);

        var firstAidRequest = TestDataBuilder<CreateSafetyIncidentRequest>.CreateRealisticSafetyIncident(_testProject.Id);
        firstAidRequest.IncidentType = "First Aid";
        var firstAidIncident = await QualitySafetyClient.CreateSafetyIncidentAsync(TestCompanyId, firstAidRequest);

        // Act - Filter by incident type
        var filteredIncidents = await ExecuteWithTrackingAsync("GetSafetyIncidents_Filtered", async () =>
        {
            return await QualitySafetyClient.GetSafetyIncidentsAsync(TestCompanyId, _testProject.Id, incidentType: "Near Miss");
        });

        // Assert
        ValidateApiResponse(filteredIncidents, "GetSafetyIncidents_Filtered");
        filteredIncidents.Should().Contain(i => i.Id == nearMissIncident.Id, "Should contain Near Miss incident");
        filteredIncidents.Should().OnlyContain(i => i.IncidentType == "Near Miss", "Should only contain Near Miss incidents");
        
        Output.WriteLine($"Filtered incidents: {filteredIncidents.Count()} Near Miss incidents");
        
        ValidatePerformance("GetSafetyIncidents_Filtered", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Quality Control Operations (Medium Priority)

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "QualityControl")]
    [Trait("Priority", "Medium")]
    public async Task GetQualityControlItems_Should_Return_Project_QC_Items()
    {
        // Act
        var qcItems = await ExecuteWithTrackingAsync("GetQualityControlItems", async () =>
        {
            return await QualitySafetyClient.GetQualityControlItemsAsync(TestCompanyId, _testProject!.Id);
        });

        // Assert
        ValidateApiResponse(qcItems, "GetQualityControlItems");
        // QC items might be empty in a new project
        
        Output.WriteLine($"Retrieved {qcItems.Count()} quality control items for project {_testProject!.Id}");
        
        ValidatePerformance("GetQualityControlItems", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Compliance and Reporting Operations (Medium Priority)

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Compliance")]
    [Trait("Priority", "Medium")]
    public async Task GetComplianceReports_Should_Return_Available_Reports()
    {
        // Act
        var reports = await ExecuteWithTrackingAsync("GetComplianceReports", async () =>
        {
            return await QualitySafetyClient.GetComplianceReportsAsync(TestCompanyId, _testProject!.Id);
        });

        // Assert
        ValidateApiResponse(reports, "GetComplianceReports");
        // Reports might be empty in a new project
        
        Output.WriteLine($"Retrieved {reports.Count()} compliance reports for project {_testProject!.Id}");
        
        ValidatePerformance("GetComplianceReports", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Operation", "Reporting")]
    [Trait("Priority", "Medium")]
    public async Task GenerateSafetyReport_Should_Create_Report_Successfully()
    {
        // Arrange
        var reportRequest = new GenerateSafetyReportRequest
        {
            ProjectId = _testProject!.Id,
            ReportType = "Incident Summary",
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.UtcNow,
            IncludeDetails = true
        };

        // Act
        var report = await ExecuteWithTrackingAsync("GenerateSafetyReport", async () =>
        {
            return await QualitySafetyClient.GenerateSafetyReportAsync(TestCompanyId, reportRequest);
        });

        // Assert
        ValidateApiResponse(report, "GenerateSafetyReport");
        report.Id.Should().BeGreaterThan(0, "Generated report should have valid ID");
        report.ReportType.Should().Be(reportRequest.ReportType, "Report type should match request");
        report.Status.Should().NotBeNullOrEmpty("Report status should be set");
        
        Output.WriteLine($"Generated safety report: {report.Id} - {report.ReportType}");
        
        ValidatePerformance("GenerateSafetyReport", TestConfig.PerformanceThresholds.BulkOperationMs);
    }

    #endregion

    #region Performance and Stress Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Focus", "Performance")]
    public async Task Bulk_Observation_Creation_Should_Handle_Multiple_Operations()
    {
        // Arrange
        const int observationCount = 10;
        var observationRequests = Enumerable.Range(0, observationCount)
            .Select(_ => TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id))
            .ToList();

        // Act
        var createdObservations = await ExecuteBatchOperationsAsync(
            "BulkObservationCreation",
            observationRequests.Select(req => 
                new Func<Task<Observation>>(() => QualitySafetyClient.CreateObservationAsync(TestCompanyId, req))),
            maxConcurrency: 3,
            delayBetweenBatches: TimeSpan.FromMilliseconds(100));

        // Assert
        createdObservations.Should().HaveCount(observationCount, "All observations should be created");
        createdObservations.Should().OnlyContain(o => o.Id > 0, "All observations should have valid IDs");
        
        Output.WriteLine($"Successfully created {createdObservations.Count} observations in bulk");
        
        ValidatePerformance("BulkObservationCreation", TestConfig.PerformanceThresholds.BulkOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Focus", "Performance")]
    public async Task Concurrent_Observation_Retrieval_Should_Handle_Load()
    {
        // Arrange - Create some test observations first
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id);
        await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);

        // Act
        const int concurrencyLevel = 5;
        var results = await ExecuteConcurrentOperationsAsync(
            "ConcurrentObservationRetrieval",
            async index =>
            {
                var observations = await QualitySafetyClient.GetObservationsAsync(TestCompanyId, _testProject.Id);
                return observations.Count();
            },
            concurrencyLevel);

        // Assert
        results.Should().HaveCount(concurrencyLevel, "All concurrent requests should succeed");
        results.Should().OnlyContain(count => count > 0, "All requests should return observations");
        
        Output.WriteLine($"Concurrent retrieval completed successfully. Observation count: {results[0]}");
        
        ValidatePerformance("ConcurrentObservationRetrieval", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Error Handling and Edge Cases

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Focus", "ErrorHandling")]
    public async Task GetObservation_With_Invalid_Id_Should_Handle_Error_Gracefully()
    {
        // Arrange
        const int invalidObservationId = 999999999;

        // Act & Assert
        await ExecuteWithTrackingAsync("GetObservation_InvalidId", async () =>
        {
            var exception = await Assert.ThrowsAsync<ProcoreApiException>(
                () => QualitySafetyClient.GetObservationAsync(TestCompanyId, invalidObservationId));

            exception.Should().NotBeNull("Should throw ProcoreApiException for invalid observation ID");
            exception.StatusCode.Should().Be(404, "Should return 404 for non-existent observation");
            
            Output.WriteLine($"Invalid observation ID handled correctly: {exception.Message}");
            
            return true;
        });
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Focus", "ErrorHandling")]
    public async Task CreateObservation_With_Invalid_Project_Should_Handle_Error()
    {
        // Arrange
        const int invalidProjectId = 999999999;
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(invalidProjectId);

        // Act & Assert
        await ExecuteWithTrackingAsync("CreateObservation_InvalidProject", async () =>
        {
            var exception = await Assert.ThrowsAsync<ProcoreApiException>(
                () => QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest));

            exception.Should().NotBeNull("Should throw ProcoreApiException for invalid project ID");
            exception.StatusCode.Should().BeOneOf(400, 404, "Should return 400 or 404 for invalid project");
            
            Output.WriteLine($"Invalid project ID handled correctly: {exception.Message}");
            
            return true;
        });
    }

    #endregion

    #region Data Validation and Type Mapping

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "QualitySafety")]
    [Trait("Focus", "TypeMapping")]
    public async Task Observation_API_Response_Should_Map_To_Correct_Types()
    {
        // Arrange - Create test observation
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(_testProject!.Id);
        var createdObservation = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);

        // Act & Assert
        await ExecuteWithTrackingAsync("ObservationTypeMappingValidation", async () =>
        {
            var observation = await QualitySafetyClient.GetObservationAsync(TestCompanyId, createdObservation.Id);
            
            ValidateObservationTypeMapping(observation);
            
            Output.WriteLine("Observation type mapping validation completed successfully");
            return true;
        });
    }

    private void ValidateObservationTypeMapping(Observation observation)
    {
        observation.Should().NotBeNull("Observation should not be null");
        observation.Id.Should().BeGreaterThan(0, "Observation ID should be positive");
        observation.Title.Should().NotBeNullOrEmpty("Observation title should not be empty");
        observation.Description.Should().NotBeNullOrEmpty("Observation description should not be empty");
        observation.ObservationType.Should().NotBeNullOrEmpty("Observation type should not be empty");
        observation.Priority.Should().NotBeNullOrEmpty("Observation priority should not be empty");
        observation.CreatedAt.Should().NotBe(default(DateTime), "Observation created date should be set");
        observation.ProjectId.Should().BeGreaterThan(0, "Project ID should be positive");
        
        if (observation.DueDate.HasValue)
        {
            observation.DueDate.Value.Should().BeAfter(observation.CreatedAt, "Due date should be after creation date");
        }
        
        if (!string.IsNullOrEmpty(observation.AssigneeEmail))
        {
            observation.AssigneeEmail.Should().Contain("@", "Assignee email should be valid");
        }
    }

    #endregion
}

// Placeholder model classes for QualitySafety operations
public class Project : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Observation : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ObservationType { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public string AssigneeEmail { get; set; } = string.Empty;
}

public class UpdateObservationRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class Inspection : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string InspectionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class CreateInspectionRequest
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string InspectionType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string InspectorEmail { get; set; } = string.Empty;
}

public class SafetyIncident : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string IncidentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string InjuredPersonName { get; set; } = string.Empty;
    public DateTime IncidentDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class QualityControlItem : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ComplianceReport : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}

public class SafetyReport : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}

public class GenerateSafetyReportRequest
{
    public int ProjectId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IncludeDetails { get; set; }
}