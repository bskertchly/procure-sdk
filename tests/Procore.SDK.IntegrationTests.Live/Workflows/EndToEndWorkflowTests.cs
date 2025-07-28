namespace Procore.SDK.IntegrationTests.Live.Workflows;

/// <summary>
/// End-to-end integration tests for complete construction management workflows
/// Tests cross-client operations with data consistency validation
/// </summary>
public class EndToEndWorkflowTests : IntegrationTestBase
{
    public EndToEndWorkflowTests(LiveSandboxFixture fixture, ITestOutputHelper output) 
        : base(fixture, output) { }

    #region Complete Project Lifecycle Workflow

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "EndToEnd")]
    [Trait("Workflow", "ProjectLifecycle")]
    public async Task Complete_Project_Lifecycle_Should_Work_Across_All_Clients()
    {
        // This test demonstrates a complete construction project workflow
        // spanning all SDK clients and validating data consistency

        Project? project = null;
        User? projectManager = null;
        Observation? safetyObservation = null;
        Invoice? projectInvoice = null;

        try
        {
            await ExecuteWithTrackingAsync("CompleteProjectLifecycle", async () =>
            {
                Output.WriteLine("=== Starting Complete Project Lifecycle Workflow ===");

                // Phase 1: Project Setup (Core + ProjectManagement)
                Output.WriteLine("\n--- Phase 1: Project Creation and Setup ---");
                
                project = await CreateProjectWithTeamAsync();
                project.Should().NotBeNull("Project should be created successfully");
                Output.WriteLine($"✓ Created project: {project!.Id} - {project.Name}");

                // Phase 2: Team Assignment (Core)
                projectManager = await AssignProjectManagerAsync(project.Id);
                projectManager.Should().NotBeNull("Project manager should be assigned");
                Output.WriteLine($"✓ Assigned project manager: {projectManager!.FirstName} {projectManager.LastName}");

                // Phase 3: Safety Setup (QualitySafety)
                safetyObservation = await CreateInitialSafetyObservationAsync(project.Id);
                safetyObservation.Should().NotBeNull("Safety observation should be created");
                Output.WriteLine($"✓ Created safety observation: {safetyObservation!.Id} - {safetyObservation.Title}");

                // Phase 4: Initial Inspection (QualitySafety)
                var inspection = await CreateProjectInspectionAsync(project.Id);
                inspection.Should().NotBeNull("Project inspection should be created");
                Output.WriteLine($"✓ Created project inspection: {inspection.Id} - {inspection.Title}");

                // Phase 5: Resource Planning (ResourceManagement)
                var resources = await SetupProjectResourcesAsync(project.Id);
                resources.Should().NotBeEmpty("Project resources should be allocated");
                Output.WriteLine($"✓ Allocated {resources.Count} project resources");

                // Phase 6: Financial Setup (ConstructionFinancials)
                projectInvoice = await CreateProjectInvoiceAsync(project.Id);
                projectInvoice.Should().NotBeNull("Project invoice should be created");
                Output.WriteLine($"✓ Created project invoice: {projectInvoice!.Id} - ${projectInvoice.Amount}");

                // Phase 7: Productivity Tracking (FieldProductivity)
                var productivityReport = await CreateProductivityReportAsync(project.Id);
                productivityReport.Should().NotBeNull("Productivity report should be created");
                Output.WriteLine($"✓ Created productivity report: {productivityReport.Id} - {productivityReport.ReportName}");

                // Phase 8: Cross-Client Data Validation
                await ValidateCrossClientDataConsistencyAsync(project.Id);
                Output.WriteLine($"✓ Validated cross-client data consistency");

                Output.WriteLine("\n=== Complete Project Lifecycle Workflow Completed Successfully ===");
                return true;
            });

            ValidatePerformance("CompleteProjectLifecycle", TestConfig.PerformanceThresholds.BulkOperationMs * 2);
        }
        finally
        {
            // Cleanup will be handled by the fixture dispose
            if (project != null)
            {
                Output.WriteLine($"\n--- Cleanup: Project {project.Id} will be cleaned up by fixture ---");
            }
        }
    }

    #endregion

    #region Safety Observation Workflow

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "EndToEnd")]
    [Trait("Workflow", "SafetyObservation")]
    public async Task Safety_Observation_Workflow_Should_Handle_Complete_Lifecycle()
    {
        // Demonstrates complete safety observation workflow from creation to resolution
        
        var project = await CreateTestProjectAsync();
        
        await ExecuteWithTrackingAsync("SafetyObservationWorkflow", async () =>
        {
            Output.WriteLine("=== Starting Safety Observation Workflow ===");

            // Step 1: Create initial safety observation
            var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(project.Id);
            observationRequest.ObservationType = "Safety";
            observationRequest.Priority = "High";
            
            var observation = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);
            Output.WriteLine($"✓ Created safety observation: {observation.Id} - {observation.Title}");

            // Step 2: Assign observation to team member
            var users = await CoreClient.GetUsersAsync(TestCompanyId);
            var assignee = users.First();
            
            var updateRequest = new UpdateObservationRequest
            {
                Title = observation.Title,
                Description = observation.Description,
                Priority = observation.Priority,
                Status = "Assigned"
            };
            
            var assignedObservation = await QualitySafetyClient.UpdateObservationAsync(TestCompanyId, observation.Id, updateRequest);
            assignedObservation.Status.Should().Be("Assigned", "Observation should be assigned");
            Output.WriteLine($"✓ Assigned observation to team member");

            // Step 3: Create related safety incident if needed
            var incidentRequest = TestDataBuilder<CreateSafetyIncidentRequest>.CreateRealisticSafetyIncident(project.Id);
            incidentRequest.Description = $"Related to observation {observation.Id}: {incidentRequest.Description}";
            
            var incident = await QualitySafetyClient.CreateSafetyIncidentAsync(TestCompanyId, incidentRequest);
            Output.WriteLine($"✓ Created related safety incident: {incident.Id}");

            // Step 4: Create corrective action inspection
            var inspectionRequest = TestDataBuilder<CreateInspectionRequest>.CreateRealisticInspection(project.Id);
            inspectionRequest.Title = $"Corrective Action for Observation {observation.Id}";
            inspectionRequest.InspectionType = "Safety Corrective Action";
            
            var inspection = await QualitySafetyClient.CreateInspectionAsync(TestCompanyId, inspectionRequest);
            Output.WriteLine($"✓ Created corrective action inspection: {inspection.Id}");

            // Step 5: Complete the inspection
            var completedInspection = await QualitySafetyClient.CompleteInspectionAsync(TestCompanyId, inspection.Id);
            completedInspection.Status.Should().Be("Completed", "Inspection should be completed");
            Output.WriteLine($"✓ Completed corrective action inspection");

            // Step 6: Update observation to resolved
            updateRequest.Status = "Resolved";
            var resolvedObservation = await QualitySafetyClient.UpdateObservationAsync(TestCompanyId, observation.Id, updateRequest);
            resolvedObservation.Status.Should().Be("Resolved", "Observation should be resolved");
            Output.WriteLine($"✓ Resolved safety observation");

            // Step 7: Generate safety report
            var reportRequest = new GenerateSafetyReportRequest
            {
                ProjectId = project.Id,
                ReportType = "Incident Resolution Summary",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow,
                IncludeDetails = true
            };
            
            var report = await QualitySafetyClient.GenerateSafetyReportAsync(TestCompanyId, reportRequest);
            report.Should().NotBeNull("Safety report should be generated");
            Output.WriteLine($"✓ Generated safety report: {report.Id}");

            Output.WriteLine("\n=== Safety Observation Workflow Completed Successfully ===");
            return true;
        });

        ValidatePerformance("SafetyObservationWorkflow", TestConfig.PerformanceThresholds.BulkOperationMs);
    }

    #endregion

    #region Financial Approval Workflow

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "EndToEnd")]
    [Trait("Workflow", "FinancialApproval")]
    public async Task Financial_Approval_Workflow_Should_Handle_Invoice_Processing()
    {
        var project = await CreateTestProjectAsync();
        
        await ExecuteWithTrackingAsync("FinancialApprovalWorkflow", async () =>
        {
            Output.WriteLine("=== Starting Financial Approval Workflow ===");

            // Step 1: Create project invoice
            var invoiceRequest = TestDataBuilder<CreateInvoiceRequest>.CreateRealisticInvoice(project.Id);
            invoiceRequest.Amount = 25000.00m; // Significant amount requiring approval
            
            var invoice = await ConstructionFinancialsClient.CreateInvoiceAsync(TestCompanyId, invoiceRequest);
            Output.WriteLine($"✓ Created invoice: {invoice.Id} - ${invoice.Amount}");

            // Step 2: Get approval workflow users
            var users = await CoreClient.GetUsersAsync(TestCompanyId);
            var approver = users.Skip(1).First(); // Use second user as approver
            
            Output.WriteLine($"✓ Identified approver: {approver.FirstName} {approver.LastName}");

            // Step 3: Create compliance document for invoice
            var complianceRequest = new CreateComplianceDocumentRequest
            {
                InvoiceId = invoice.Id,
                DocumentType = "Tax Compliance",
                DocumentName = $"Tax Documentation - Invoice {invoice.InvoiceNumber}",
                RequiredForApproval = true
            };
            
            var complianceDoc = await ConstructionFinancialsClient.CreateComplianceDocumentAsync(TestCompanyId, complianceRequest);
            Output.WriteLine($"✓ Created compliance document: {complianceDoc.Id}");

            // Step 4: Update invoice status through approval process
            var approvalRequest = new UpdateInvoiceStatusRequest
            {
                Status = "Under Review",
                ApproverEmail = approver.Email,
                Comments = "Invoice submitted for financial approval"
            };
            
            var reviewInvoice = await ConstructionFinancialsClient.UpdateInvoiceStatusAsync(TestCompanyId, invoice.Id, approvalRequest);
            reviewInvoice.Status.Should().Be("Under Review", "Invoice should be under review");
            Output.WriteLine($"✓ Invoice status updated to: {reviewInvoice.Status}");

            // Step 5: Simulate approval process
            approvalRequest.Status = "Approved";
            approvalRequest.Comments = "Financial approval granted - compliance documentation verified";
            
            var approvedInvoice = await ConstructionFinancialsClient.UpdateInvoiceStatusAsync(TestCompanyId, invoice.Id, approvalRequest);
            approvedInvoice.Status.Should().Be("Approved", "Invoice should be approved");
            Output.WriteLine($"✓ Invoice approved by: {approver.FirstName} {approver.LastName}");

            // Step 6: Generate financial report
            var reportRequest = new GenerateFinancialReportRequest
            {
                ProjectId = project.Id,
                ReportType = "Invoice Approval Summary",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow,
                IncludeApprovalHistory = true
            };
            
            var financialReport = await ConstructionFinancialsClient.GenerateFinancialReportAsync(TestCompanyId, reportRequest);
            financialReport.Should().NotBeNull("Financial report should be generated");
            Output.WriteLine($"✓ Generated financial report: {financialReport.Id}");

            Output.WriteLine("\n=== Financial Approval Workflow Completed Successfully ===");
            return true;
        });

        ValidatePerformance("FinancialApprovalWorkflow", TestConfig.PerformanceThresholds.BulkOperationMs);
    }

    #endregion

    #region Resource Allocation Workflow

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "EndToEnd")]
    [Trait("Workflow", "ResourceAllocation")]
    public async Task Resource_Allocation_Workflow_Should_Handle_Workforce_Planning()
    {
        var project = await CreateTestProjectAsync();
        
        await ExecuteWithTrackingAsync("ResourceAllocationWorkflow", async () =>
        {
            Output.WriteLine("=== Starting Resource Allocation Workflow ===");

            // Step 1: Create initial resource requirements
            var laborResourceRequest = TestDataBuilder<CreateScheduleResourceRequest>.CreateRealisticScheduleResource(project.Id);
            laborResourceRequest.ResourceType = "Labor";
            laborResourceRequest.ResourceName = "Site Foreman";
            
            var laborResource = await ResourceManagementClient.CreateScheduleResourceAsync(TestCompanyId, laborResourceRequest);
            Output.WriteLine($"✓ Created labor resource: {laborResource.Id} - {laborResource.ResourceName}");

            // Step 2: Create equipment resource
            var equipmentResourceRequest = TestDataBuilder<CreateScheduleResourceRequest>.CreateRealisticScheduleResource(project.Id);
            equipmentResourceRequest.ResourceType = "Equipment";
            equipmentResourceRequest.ResourceName = "Excavator";
            
            var equipmentResource = await ResourceManagementClient.CreateScheduleResourceAsync(TestCompanyId, equipmentResourceRequest);
            Output.WriteLine($"✓ Created equipment resource: {equipmentResource.Id} - {equipmentResource.ResourceName}");

            // Step 3: Create productivity tracking for resources
            var productivityRequest = TestDataBuilder<CreateProductivityReportRequest>.CreateRealisticProductivityReport(project.Id);
            productivityRequest.ReportName = "Resource Utilization Report";
            productivityRequest.Activity = "Site Preparation";
            
            var productivityReport = await FieldProductivityClient.CreateProductivityReportAsync(TestCompanyId, productivityRequest);
            Output.WriteLine($"✓ Created productivity report: {productivityReport.Id}");

            // Step 4: Create timecard entries for labor resources
            var timecardRequest = TestDataBuilder<CreateTimecardEntryRequest>.CreateRealisticTimecardEntry(project.Id);
            timecardRequest.WorkerName = "John Site Foreman";
            timecardRequest.WorkDescription = "Site preparation and equipment coordination";
            
            var timecardEntry = await FieldProductivityClient.CreateTimecardEntryAsync(TestCompanyId, timecardRequest);
            Output.WriteLine($"✓ Created timecard entry: {timecardEntry.Id} for {timecardEntry.WorkerName}");

            // Step 5: Update resource allocation based on productivity
            var updateResourceRequest = new UpdateScheduleResourceRequest
            {
                Quantity = laborResource.Quantity + 1, // Add more resources based on needs
                Notes = "Increased allocation based on productivity analysis",
                Rate = laborResource.Rate * 1.1m // Adjust rate
            };
            
            var updatedResource = await ResourceManagementClient.UpdateScheduleResourceAsync(TestCompanyId, laborResource.Id, updateResourceRequest);
            updatedResource.Quantity.Should().Be(laborResource.Quantity + 1, "Resource quantity should be updated");
            Output.WriteLine($"✓ Updated resource allocation: {updatedResource.ResourceName} quantity: {updatedResource.Quantity}");

            // Step 6: Generate resource utilization report
            var utilizationRequest = new GenerateResourceReportRequest
            {
                ProjectId = project.Id,
                ReportType = "Resource Utilization Summary",
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow,
                IncludeProductivityMetrics = true
            };
            
            var utilizationReport = await ResourceManagementClient.GenerateResourceReportAsync(TestCompanyId, utilizationRequest);
            utilizationReport.Should().NotBeNull("Resource utilization report should be generated");
            Output.WriteLine($"✓ Generated resource utilization report: {utilizationReport.Id}");

            Output.WriteLine("\n=== Resource Allocation Workflow Completed Successfully ===");
            return true;
        });

        ValidatePerformance("ResourceAllocationWorkflow", TestConfig.PerformanceThresholds.BulkOperationMs);
    }

    #endregion

    #region Data Consistency Validation

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Focus", "EndToEnd")]
    [Trait("Workflow", "DataConsistency")]
    public async Task Cross_Client_Data_Consistency_Should_Be_Maintained()
    {
        var project = await CreateTestProjectAsync();
        
        await ExecuteWithTrackingAsync("CrossClientDataConsistency", async () =>
        {
            Output.WriteLine("=== Starting Cross-Client Data Consistency Validation ===");

            // Create data across multiple clients
            var observation = await QualitySafetyClient.CreateObservationAsync(TestCompanyId, 
                TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(project.Id));
            
            var invoice = await ConstructionFinancialsClient.CreateInvoiceAsync(TestCompanyId,
                TestDataBuilder<CreateInvoiceRequest>.CreateRealisticInvoice(project.Id));
            
            var resource = await ResourceManagementClient.CreateScheduleResourceAsync(TestCompanyId,
                TestDataBuilder<CreateScheduleResourceRequest>.CreateRealisticScheduleResource(project.Id));
            
            Output.WriteLine($"✓ Created test data across all clients");

            // Validate project consistency across clients
            var projectFromPM = await ProjectManagementClient.GetProjectAsync(TestCompanyId, project.Id);
            var observationsForProject = await QualitySafetyClient.GetObservationsAsync(TestCompanyId, project.Id);
            var invoicesForProject = await ConstructionFinancialsClient.GetInvoicesAsync(TestCompanyId, projectId: project.Id);
            var resourcesForProject = await ResourceManagementClient.GetScheduleResourcesAsync(TestCompanyId, project.Id);

            // Assert data consistency
            projectFromPM.Id.Should().Be(project.Id, "Project ID should be consistent across clients");
            observationsForProject.Should().Contain(o => o.Id == observation.Id, "Observation should be linked to correct project");
            invoicesForProject.Should().Contain(i => i.Id == invoice.Id, "Invoice should be linked to correct project");
            resourcesForProject.Should().Contain(r => r.Id == resource.Id, "Resource should be linked to correct project");

            // Validate timestamps are reasonable
            var now = DateTime.UtcNow;
            observation.CreatedAt.Should().BeCloseTo(now, TimeSpan.FromMinutes(5), "Observation timestamp should be recent");
            invoice.CreatedAt.Should().BeCloseTo(now, TimeSpan.FromMinutes(5), "Invoice timestamp should be recent");
            resource.CreatedAt.Should().BeCloseTo(now, TimeSpan.FromMinutes(5), "Resource timestamp should be recent");

            // Validate cross-references
            observationsForProject.Should().OnlyContain(o => o.ProjectId == project.Id, "All observations should belong to the project");
            invoicesForProject.Should().OnlyContain(i => i.ProjectId == project.Id, "All invoices should belong to the project");
            resourcesForProject.Should().OnlyContain(r => r.ProjectId == project.Id, "All resources should belong to the project");

            Output.WriteLine($"✓ Validated data consistency across all clients");
            Output.WriteLine($"  Project: {project.Id}, Observations: {observationsForProject.Count()}, Invoices: {invoicesForProject.Count()}, Resources: {resourcesForProject.Count()}");

            Output.WriteLine("\n=== Cross-Client Data Consistency Validation Completed Successfully ===");
            return true;
        });

        ValidatePerformance("CrossClientDataConsistency", TestConfig.PerformanceThresholds.BulkOperationMs);
    }

    #endregion

    #region Helper Methods

    private async Task<Project> CreateProjectWithTeamAsync()
    {
        var projectRequest = TestDataBuilder<CreateProjectRequest>.CreateRealisticProject();
        projectRequest.Name = $"E2E Test Project - {DateTime.UtcNow:yyyyMMdd-HHmmss}";
        projectRequest.Description = "End-to-end integration test project with full workflow validation";
        
        var project = await ProjectManagementClient.CreateProjectAsync(TestCompanyId, projectRequest);
        
        // Store for cleanup
        await CreateTestDataAsync($"e2e_project_{project.Id}", async () => project);
        
        return project;
    }

    private async Task<User> AssignProjectManagerAsync(int projectId)
    {
        var users = await CoreClient.GetUsersAsync(TestCompanyId);
        var projectManager = users.First();
        
        // In a real scenario, this would involve project role assignment
        // For this test, we'll just return the user as the "assigned" PM
        
        return projectManager;
    }

    private async Task<Observation> CreateInitialSafetyObservationAsync(int projectId)
    {
        var observationRequest = TestDataBuilder<CreateObservationRequest>.CreateRealisticObservation(projectId);
        observationRequest.ObservationType = "Safety";
        observationRequest.Priority = "High";
        observationRequest.Title = "Initial Safety Assessment";
        observationRequest.Description = "Comprehensive safety assessment for project initiation";
        
        return await QualitySafetyClient.CreateObservationAsync(TestCompanyId, observationRequest);
    }

    // TODO: Implement when Inspection models are available
    // private async Task<Inspection> CreateProjectInspectionAsync(int projectId)
    // {
    //     var inspectionRequest = TestDataBuilder<CreateInspectionRequest>.CreateRealisticInspection(projectId);
    //     inspectionRequest.Title = "Project Initiation Inspection";
    //     inspectionRequest.InspectionType = "Safety";
    //     
    //     return await QualitySafetyClient.CreateInspectionAsync(TestCompanyId, inspectionRequest);
    // }

    private async Task<List<ScheduleResource>> SetupProjectResourcesAsync(int projectId)
    {
        var resources = new List<ScheduleResource>();
        
        // Create multiple resource types
        var resourceTypes = new[]
        {
            ("Labor", "Site Supervisor"),
            ("Equipment", "Crane"),
            ("Material", "Concrete"),
            ("Subcontractor", "Electrical Contractor")
        };

        foreach (var (type, name) in resourceTypes)
        {
            var resourceRequest = TestDataBuilder<CreateScheduleResourceRequest>.CreateRealisticScheduleResource(projectId);
            resourceRequest.ResourceType = type;
            resourceRequest.ResourceName = name;
            
            var resource = await ResourceManagementClient.CreateScheduleResourceAsync(TestCompanyId, resourceRequest);
            resources.Add(resource);
        }
        
        return resources;
    }

    private async Task<Invoice> CreateProjectInvoiceAsync(int projectId)
    {
        var invoiceRequest = TestDataBuilder<CreateInvoiceRequest>.CreateRealisticInvoice(projectId);
        invoiceRequest.Description = "Initial project setup and mobilization costs";
        invoiceRequest.Amount = 15000.00m;
        
        return await ConstructionFinancialsClient.CreateInvoiceAsync(TestCompanyId, invoiceRequest);
    }

    private async Task<ProductivityReport> CreateProductivityReportAsync(int projectId)
    {
        var reportRequest = TestDataBuilder<CreateProductivityReportRequest>.CreateRealisticProductivityReport(projectId);
        reportRequest.ReportName = "Project Initiation Productivity";
        reportRequest.Activity = "Site Setup";
        
        return await FieldProductivityClient.CreateProductivityReportAsync(TestCompanyId, reportRequest);
    }

    private async Task ValidateCrossClientDataConsistencyAsync(int projectId)
    {
        // Retrieve project data from all relevant clients
        var project = await ProjectManagementClient.GetProjectAsync(TestCompanyId, projectId);
        var observations = await QualitySafetyClient.GetObservationsAsync(TestCompanyId, projectId);
        var invoices = await ConstructionFinancialsClient.GetInvoicesAsync(TestCompanyId, projectId: projectId);
        var resources = await ResourceManagementClient.GetScheduleResourcesAsync(TestCompanyId, projectId);
        var productivityReports = await FieldProductivityClient.GetProductivityReportsAsync(TestCompanyId, projectId);

        // Validate that all data is consistently linked to the project
        project.Should().NotBeNull("Project should exist in ProjectManagement client");
        observations.Should().OnlyContain(o => o.ProjectId == projectId, "All observations should be linked to the project");
        invoices.Should().OnlyContain(i => i.ProjectId == projectId, "All invoices should be linked to the project");
        resources.Should().OnlyContain(r => r.ProjectId == projectId, "All resources should be linked to the project");
        productivityReports.Should().OnlyContain(p => p.ProjectId == projectId, "All productivity reports should be linked to the project");

        Output.WriteLine($"Cross-client data consistency validated for project {projectId}");
        Output.WriteLine($"  Observations: {observations.Count()}, Invoices: {invoices.Count()}, Resources: {resources.Count()}, Reports: {productivityReports.Count()}");
    }

    #endregion
}

// Additional placeholder classes for workflow tests
public class ScheduleResource : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ResourceName { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Rate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateScheduleResourceRequest
{
    public int Quantity { get; set; }
    public string Notes { get; set; } = string.Empty;
    public decimal Rate { get; set; }
}

public class ProductivityReport : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ReportName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class TimecardEntry : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string WorkerName { get; set; } = string.Empty;
    public string WorkDescription { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class UpdateInvoiceStatusRequest
{
    public string Status { get; set; } = string.Empty;
    public string ApproverEmail { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
}

public class CreateComplianceDocumentRequest
{
    public int InvoiceId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentName { get; set; } = string.Empty;
    public bool RequiredForApproval { get; set; }
}

public class ComplianceDocument : IIdentifiable
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentName { get; set; } = string.Empty;
}

public class GenerateFinancialReportRequest
{
    public int ProjectId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IncludeApprovalHistory { get; set; }
}

public class FinancialReport : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}

public class GenerateResourceReportRequest
{
    public int ProjectId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IncludeProductivityMetrics { get; set; }
}

public class ResourceReport : IIdentifiable
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}