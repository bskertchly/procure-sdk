using Bogus;
using Microsoft.Extensions.Logging;

namespace Procore.SDK.IntegrationTests.Live.Infrastructure;

/// <summary>
/// Builder for creating realistic test data using Bogus framework
/// </summary>
/// <typeparam name="T">Type of test data to build</typeparam>
public class TestDataBuilder<T> where T : class
{
    private readonly ILogger<TestDataBuilder<T>> _logger;
    private readonly Faker _faker;

    public TestDataBuilder(ILogger<TestDataBuilder<T>> logger)
    {
        _logger = logger;
        _faker = new Faker();
    }

    /// <summary>
    /// Creates realistic project data
    /// </summary>
    public static CreateProjectRequest CreateRealisticProject()
    {
        var faker = new Faker();
        var projectTypes = new[] { "Commercial", "Residential", "Industrial", "Infrastructure", "Institutional" };
        
        return new CreateProjectRequest
        {
            Name = $"{faker.Company.CompanyName()} {faker.Random.Word()} Project",
            ProjectNumber = faker.Random.Replace("PRJ-####-###"),
            Description = faker.Lorem.Paragraph(2),
            ProjectOwnerEmailAddress = faker.Internet.Email(),
            Address = faker.Address.FullAddress(),
            ProjectType = faker.Random.ListItem(projectTypes),
            EstimatedValue = faker.Random.Decimal(100000, 50000000),
            StartDate = faker.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(30)),
            EstimatedCompletionDate = faker.Date.Between(DateTime.UtcNow.AddDays(90), DateTime.UtcNow.AddDays(365))
        };
    }

    /// <summary>
    /// Creates realistic observation data
    /// </summary>
    public static CreateObservationRequest CreateRealisticObservation(int projectId)
    {
        var faker = new Faker();
        var observationTypes = new[] { "Safety", "Quality", "Environmental", "General" };
        var priorities = new[] { "Low", "Medium", "High", "Critical" };
        
        return new CreateObservationRequest
        {
            ProjectId = projectId,
            Title = faker.Lorem.Sentence(4),
            Description = faker.Lorem.Paragraph(3),
            ObservationType = faker.Random.ListItem(observationTypes),
            Priority = faker.Random.ListItem(priorities),
            Location = faker.Address.StreetAddress(),
            ObservedAt = faker.Date.Between(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow),
            DueDate = faker.Date.Between(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(30)),
            AssigneeEmail = faker.Internet.Email()
        };
    }

    /// <summary>
    /// Creates realistic drawing data
    /// </summary>
    public static CreateDrawingRequest CreateRealisticDrawing(int projectId)
    {
        var faker = new Faker();
        var drawingTypes = new[] { "Architectural", "Structural", "Mechanical", "Electrical", "Plumbing" };
        var disciplines = new[] { "Architecture", "Structural Engineering", "MEP", "Civil" };
        
        return new CreateDrawingRequest
        {
            ProjectId = projectId,
            Title = $"{faker.Random.ListItem(drawingTypes)} Drawing - {faker.Lorem.Word()}",
            Number = faker.Random.Replace("D-####"),
            Description = faker.Lorem.Sentence(),
            Discipline = faker.Random.ListItem(disciplines),
            DrawingType = faker.Random.ListItem(drawingTypes),
            Revision = faker.Random.Int(1, 10).ToString(),
            Scale = faker.Random.ListItem(new[] { "1/4\"=1'", "1/8\"=1'", "1\"=10'", "1\"=20'" }),
            SheetSize = faker.Random.ListItem(new[] { "11x17", "24x36", "30x42", "36x48" })
        };
    }

    /// <summary>
    /// Creates realistic meeting data
    /// </summary>
    public static CreateMeetingRequest CreateRealisticMeeting(int projectId)
    {
        var faker = new Faker();
        var meetingTypes = new[] { "Project Review", "Safety Meeting", "Progress Review", "Coordination", "Design Review" };
        var locations = new[] { "Job Site Trailer", "Conference Room", "Virtual", "Job Site", "Main Office" };
        
        return new CreateMeetingRequest
        {
            ProjectId = projectId,
            Title = faker.Random.ListItem(meetingTypes),
            Description = faker.Lorem.Paragraph(),
            Location = faker.Random.ListItem(locations),
            StartTime = faker.Date.Between(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(14)),
            Duration = TimeSpan.FromMinutes(faker.Random.Int(30, 120)),
            MeetingType = faker.Random.ListItem(meetingTypes),
            IsPrivate = faker.Random.Bool(0.2f), // 20% chance of private meetings
            Attendees = GenerateAttendeeEmails(faker.Random.Int(3, 8))
        };
    }

    /// <summary>
    /// Creates realistic RFI data
    /// </summary>
    public static CreateRfiRequest CreateRealisticRfi(int projectId)
    {
        var faker = new Faker();
        var priorities = new[] { "Low", "Medium", "High", "Urgent" };
        var categories = new[] { "Architectural", "Structural", "MEP", "Site Work", "General" };
        
        return new CreateRfiRequest
        {
            ProjectId = projectId,
            Subject = faker.Lorem.Sentence(6),
            Question = faker.Lorem.Paragraph(2),
            Priority = faker.Random.ListItem(priorities),
            Category = faker.Random.ListItem(categories),
            DueDate = faker.Date.Between(DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddDays(21)),
            AssigneeEmail = faker.Internet.Email(),
            Location = faker.Address.StreetAddress()
        };
    }

    /// <summary>
    /// Creates realistic invoice data
    /// </summary>
    public static CreateInvoiceRequest CreateRealisticInvoice(int projectId)
    {
        var faker = new Faker();
        var invoiceTypes = new[] { "Progress", "Final", "Change Order", "Material", "Labor" };
        
        return new CreateInvoiceRequest
        {
            ProjectId = projectId,
            InvoiceNumber = faker.Random.Replace("INV-####-###"),
            InvoiceType = faker.Random.ListItem(invoiceTypes),
            Description = faker.Lorem.Sentence(),
            Amount = faker.Random.Decimal(1000, 100000),
            InvoiceDate = faker.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow),
            DueDate = faker.Date.Between(DateTime.UtcNow.AddDays(15), DateTime.UtcNow.AddDays(45)),
            VendorName = faker.Company.CompanyName(),
            VendorEmail = faker.Internet.Email()
        };
    }

    /// <summary>
    /// Creates realistic productivity report data
    /// </summary>
    public static CreateProductivityReportRequest CreateRealisticProductivityReport(int projectId)
    {
        var faker = new Faker();
        var reportTypes = new[] { "Daily", "Weekly", "Monthly", "Phase Summary" };
        var activities = new[] { "Concrete", "Framing", "Electrical", "Plumbing", "Finishing" };
        
        return new CreateProductivityReportRequest
        {
            ProjectId = projectId,
            ReportName = $"{faker.Random.ListItem(reportTypes)} Productivity Report",
            ReportType = faker.Random.ListItem(reportTypes),
            ReportDate = faker.Date.Between(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow),
            Activity = faker.Random.ListItem(activities),
            UnitsCompleted = faker.Random.Decimal(10, 1000),
            LaborHours = faker.Random.Decimal(8, 200),
            CrewSize = faker.Random.Int(2, 12),
            Weather = faker.Random.ListItem(new[] { "Clear", "Cloudy", "Light Rain", "Heavy Rain", "Snow" }),
            Temperature = faker.Random.Int(20, 95)
        };
    }

    /// <summary>
    /// Creates realistic timecard entry data
    /// </summary>
    public static CreateTimecardEntryRequest CreateRealisticTimecardEntry(int projectId)
    {
        var faker = new Faker();
        var costCodes = new[] { "01-100", "02-200", "03-300", "04-400", "05-500" };
        
        return new CreateTimecardEntryRequest
        {
            ProjectId = projectId,
            WorkerName = faker.Name.FullName(),
            WorkerEmail = faker.Internet.Email(),
            Date = faker.Date.Between(DateTime.UtcNow.AddDays(-14), DateTime.UtcNow),
            StartTime = TimeSpan.FromHours(faker.Random.Int(6, 8)),
            EndTime = TimeSpan.FromHours(faker.Random.Int(16, 18)),
            BreakDuration = TimeSpan.FromMinutes(faker.Random.Int(30, 60)),
            CostCode = faker.Random.ListItem(costCodes),
            WorkDescription = faker.Lorem.Sentence(),
            OvertimeHours = faker.Random.Decimal(0, 4)
        };
    }

    /// <summary>
    /// Creates realistic schedule resource data
    /// </summary>
    public static CreateScheduleResourceRequest CreateRealisticScheduleResource(int projectId)
    {
        var faker = new Faker();
        var resourceTypes = new[] { "Labor", "Equipment", "Material", "Subcontractor" };
        var trades = new[] { "Carpenter", "Electrician", "Plumber", "Concrete Worker", "Roofer" };
        
        return new CreateScheduleResourceRequest
        {
            ProjectId = projectId,
            ResourceName = faker.Random.ListItem(trades),
            ResourceType = faker.Random.ListItem(resourceTypes),
            Quantity = faker.Random.Int(1, 10),
            Unit = faker.Random.ListItem(new[] { "Each", "Hour", "Day", "Week" }),
            Rate = faker.Random.Decimal(25, 150),
            StartDate = faker.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddDays(30)),
            EndDate = faker.Date.Between(DateTime.UtcNow.AddDays(31), DateTime.UtcNow.AddDays(180)),
            Notes = faker.Lorem.Sentence()
        };
    }

    /// <summary>
    /// Creates a batch of realistic test data
    /// </summary>
    public static List<T> CreateBatch<TRequest>(int count, Func<TRequest> factory) where TRequest : class
    {
        var results = new List<T>();
        for (int i = 0; i < count; i++)
        {
            var item = factory();
            results.Add((T)(object)item);
        }
        return results;
    }

    /// <summary>
    /// Creates realistic safety incident data
    /// </summary>
    public static CreateSafetyIncidentRequest CreateRealisticSafetyIncident(int projectId)
    {
        var faker = new Faker();
        var incidentTypes = new[] { "Near Miss", "First Aid", "Medical Treatment", "Lost Time", "Property Damage" };
        var bodyParts = new[] { "Head", "Back", "Hand", "Foot", "Arm", "Leg", "Eye", "Other" };
        var causes = new[] { "Slip/Fall", "Struck By", "Caught In/Between", "Electrical", "Chemical", "Other" };
        
        return new CreateSafetyIncidentRequest
        {
            ProjectId = projectId,
            IncidentType = faker.Random.ListItem(incidentTypes),
            Description = faker.Lorem.Paragraph(2),
            Location = faker.Address.StreetAddress(),
            IncidentDate = faker.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow),
            InjuredPersonName = faker.Name.FullName(),
            BodyPartAffected = faker.Random.ListItem(bodyParts),
            PotentialCause = faker.Random.ListItem(causes),
            WitnessName = faker.Name.FullName(),
            ReportedBy = faker.Name.FullName(),
            ImmediateActions = faker.Lorem.Sentence(),
            PreventiveActions = faker.Lorem.Sentence()
        };
    }

    private static List<string> GenerateAttendeeEmails(int count)
    {
        var faker = new Faker();
        var emails = new List<string>();
        
        for (int i = 0; i < count; i++)
        {
            emails.Add(faker.Internet.Email());
        }
        
        return emails;
    }
}

// Request classes would be defined based on the actual SDK models
// These are placeholder definitions for the data builder

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string ProjectNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProjectOwnerEmailAddress { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public decimal EstimatedValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EstimatedCompletionDate { get; set; }
}

public class CreateObservationRequest
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ObservationType { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime ObservedAt { get; set; }
    public DateTime DueDate { get; set; }
    public string AssigneeEmail { get; set; } = string.Empty;
}

public class CreateDrawingRequest
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Discipline { get; set; } = string.Empty;
    public string DrawingType { get; set; } = string.Empty;
    public string Revision { get; set; } = string.Empty;
    public string Scale { get; set; } = string.Empty;
    public string SheetSize { get; set; } = string.Empty;
}

public class CreateMeetingRequest
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string MeetingType { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public List<string> Attendees { get; set; } = new();
}

public class CreateRfiRequest
{
    public int ProjectId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string AssigneeEmail { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}

public class CreateInvoiceRequest
{
    public int ProjectId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string InvoiceType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string VendorEmail { get; set; } = string.Empty;
}

public class CreateProductivityReportRequest
{
    public int ProjectId { get; set; }
    public string ReportName { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string Activity { get; set; } = string.Empty;
    public decimal UnitsCompleted { get; set; }
    public decimal LaborHours { get; set; }
    public int CrewSize { get; set; }
    public string Weather { get; set; } = string.Empty;
    public int Temperature { get; set; }
}

public class CreateTimecardEntryRequest
{    
    public int ProjectId { get; set; }
    public string WorkerName { get; set; } = string.Empty;
    public string WorkerEmail { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan BreakDuration { get; set; }
    public string CostCode { get; set; } = string.Empty;
    public string WorkDescription { get; set; } = string.Empty;
    public decimal OvertimeHours { get; set; }
}

public class CreateScheduleResourceRequest
{
    public int ProjectId { get; set; }
    public string ResourceName { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreateSafetyIncidentRequest
{
    public int ProjectId { get; set; }
    public string IncidentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime IncidentDate { get; set; }
    public string InjuredPersonName { get; set; } = string.Empty;
    public string BodyPartAffected { get; set; } = string.Empty;
    public string PotentialCause { get; set; } = string.Empty;
    public string WitnessName { get; set; } = string.Empty;
    public string ReportedBy { get; set; } = string.Empty;
    public string ImmediateActions { get; set; } = string.Empty;
    public string PreventiveActions { get; set; } = string.Empty;
}