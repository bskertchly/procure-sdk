namespace Procore.SDK.IntegrationTests.Live.Clients;

/// <summary>
/// Integration tests for Core Client operations against live Procore sandbox
/// Tests companies, users, documents, and custom fields operations
/// </summary>
public class CoreClientIntegrationTests : IntegrationTestBase
{
    public CoreClientIntegrationTests(LiveSandboxFixture fixture, ITestOutputHelper output) 
        : base(fixture, output) { }

    #region Company Operations

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Companies")]
    public async Task GetCompanies_Should_Return_Valid_Company_List()
    {
        // Act
        var companies = await ExecuteWithTrackingAsync("GetCompanies", async () =>
        {
            return await CoreClient.GetCompaniesAsync();
        });

        // Assert
        ValidateApiResponse(companies, "GetCompanies");
        companies.Should().NotBeEmpty("Sandbox should have at least one company");
        
        var firstCompany = companies.First();
        firstCompany.Id.Should().BeGreaterThan(0, "Company should have valid ID");
        firstCompany.Name.Should().NotBeNullOrEmpty("Company should have name");
        
        Output.WriteLine($"Retrieved {companies.Count()} companies. First company: {firstCompany.Id} - {firstCompany.Name}");
        
        // Validate performance
        ValidatePerformance("GetCompanies", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Companies")]
    public async Task GetCompany_Should_Return_Specific_Company_Details()
    {
        // Arrange
        var companies = await CoreClient.GetCompaniesAsync();
        var targetCompanyId = companies.First().Id;

        // Act
        var company = await ExecuteWithTrackingAsync("GetCompany", async () =>
        {
            return await CoreClient.GetCompanyAsync(targetCompanyId);
        });

        // Assert
        ValidateApiResponse(company, "GetCompany");
        company.Id.Should().Be(targetCompanyId, "Should return requested company");
        company.Name.Should().NotBeNullOrEmpty("Company should have name");
        
        Output.WriteLine($"Retrieved company details: {company.Id} - {company.Name}");
        
        ValidatePerformance("GetCompany", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Companies")]
    public async Task GetCompanies_With_Pagination_Should_Handle_Large_Datasets()
    {
        // Act
        var allCompanies = new List<Company>();
        int page = 1;
        const int pageSize = 10;

        await ExecuteWithTrackingAsync("GetCompanies_Paginated", async () =>
        {
            while (true)
            {
                var companies = await CoreClient.GetCompaniesAsync(page: page, pageSize: pageSize);
                
                if (!companies.Any())
                    break;
                    
                allCompanies.AddRange(companies);
                page++;
                
                // Safety check to prevent infinite loops
                if (page > 10) 
                    break;
            }
            
            return allCompanies.Count;
        });

        // Assert
        allCompanies.Should().NotBeEmpty("Should retrieve at least some companies");
        Output.WriteLine($"Retrieved {allCompanies.Count} companies across {page - 1} pages");
        
        ValidatePerformance("GetCompanies_Paginated", TestConfig.PerformanceThresholds.ApiOperationMs * 3);
    }

    #endregion

    #region User Operations

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Users")]
    public async Task GetCurrentUser_Should_Return_Authenticated_User_Details()
    {
        // Act
        var currentUser = await ExecuteWithTrackingAsync("GetCurrentUser", async () =>
        {
            return await CoreClient.GetCurrentUserAsync();
        });

        // Assert
        ValidateApiResponse(currentUser, "GetCurrentUser");
        currentUser.Id.Should().BeGreaterThan(0, "User should have valid ID");
        currentUser.Email.Should().NotBeNullOrEmpty("User should have email");
        currentUser.FirstName.Should().NotBeNullOrEmpty("User should have first name");
        currentUser.LastName.Should().NotBeNullOrEmpty("User should have last name");
        
        Output.WriteLine($"Current user: {currentUser.Id} - {currentUser.FirstName} {currentUser.LastName} ({currentUser.Email})");
        
        ValidatePerformance("GetCurrentUser", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Users")]
    public async Task GetCompanyUsers_Should_Return_Company_Directory()
    {
        // Act
        var users = await ExecuteWithTrackingAsync("GetCompanyUsers", async () =>
        {
            return await CoreClient.GetUsersAsync(TestCompanyId);
        });

        // Assert
        ValidateApiResponse(users, "GetCompanyUsers");
        users.Should().NotBeEmpty("Company should have at least one user");
        
        var firstUser = users.First();
        firstUser.Id.Should().BeGreaterThan(0, "User should have valid ID");
        firstUser.Email.Should().NotBeNullOrEmpty("User should have email");
        
        Output.WriteLine($"Retrieved {users.Count()} users for company {TestCompanyId}");
        
        ValidatePerformance("GetCompanyUsers", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Users")]
    public async Task GetUser_Should_Return_Specific_User_Details()
    {
        // Arrange
        var users = await CoreClient.GetUsersAsync(TestCompanyId);
        var targetUserId = users.First().Id;

        // Act
        var user = await ExecuteWithTrackingAsync("GetUser", async () =>
        {
            return await CoreClient.GetUserAsync(TestCompanyId, targetUserId);
        });

        // Assert
        ValidateApiResponse(user, "GetUser");
        user.Id.Should().Be(targetUserId, "Should return requested user");
        user.Email.Should().NotBeNullOrEmpty("User should have email");
        
        Output.WriteLine($"Retrieved user details: {user.Id} - {user.FirstName} {user.LastName}");
        
        ValidatePerformance("GetUser", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Document Operations

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Documents")]
    public async Task GetDocuments_Should_Return_Company_Documents()
    {
        // Act
        var documents = await ExecuteWithTrackingAsync("GetDocuments", async () =>
        {
            return await CoreClient.GetDocumentsAsync(TestCompanyId);
        });

        // Assert
        ValidateApiResponse(documents, "GetDocuments");
        // Note: Documents might be empty in a new sandbox, so we don't require any
        
        Output.WriteLine($"Retrieved {documents.Count()} documents for company {TestCompanyId}");
        
        if (documents.Any())
        {
            var firstDocument = documents.First();
            firstDocument.Id.Should().BeGreaterThan(0, "Document should have valid ID");
            firstDocument.Name.Should().NotBeNullOrEmpty("Document should have name");
        }
        
        ValidatePerformance("GetDocuments", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "Documents")]
    public async Task GetDocumentFolders_Should_Return_Folder_Structure()
    {
        // Act
        var folders = await ExecuteWithTrackingAsync("GetDocumentFolders", async () =>
        {
            return await CoreClient.GetDocumentFoldersAsync(TestCompanyId);
        });

        // Assert
        ValidateApiResponse(folders, "GetDocumentFolders");
        // Folders might be empty in a new sandbox
        
        Output.WriteLine($"Retrieved {folders.Count()} document folders for company {TestCompanyId}");
        
        if (folders.Any())
        {
            var firstFolder = folders.First();
            firstFolder.Id.Should().BeGreaterThan(0, "Folder should have valid ID");
            firstFolder.Name.Should().NotBeNullOrEmpty("Folder should have name");
        }
        
        ValidatePerformance("GetDocumentFolders", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Custom Fields Operations

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Operation", "CustomFields")]
    public async Task GetCustomFieldDefinitions_Should_Return_Field_Definitions()
    {
        // Act
        var customFields = await ExecuteWithTrackingAsync("GetCustomFieldDefinitions", async () =>
        {
            return await CoreClient.GetCustomFieldDefinitionsAsync(TestCompanyId);
        });

        // Assert
        ValidateApiResponse(customFields, "GetCustomFieldDefinitions");
        // Custom fields might be empty in a new sandbox
        
        Output.WriteLine($"Retrieved {customFields.Count()} custom field definitions for company {TestCompanyId}");
        
        if (customFields.Any())
        {
            var firstField = customFields.First();
            firstField.Id.Should().BeGreaterThan(0, "Custom field should have valid ID");
            firstField.Name.Should().NotBeNullOrEmpty("Custom field should have name");
            firstField.DataType.Should().NotBeNullOrEmpty("Custom field should have data type");
        }
        
        ValidatePerformance("GetCustomFieldDefinitions", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Focus", "ErrorHandling")]
    public async Task GetCompany_With_Invalid_Id_Should_Handle_Error_Gracefully()
    {
        // Arrange
        const int invalidCompanyId = 999999999;

        // Act & Assert
        await ExecuteWithTrackingAsync("GetCompany_InvalidId", async () =>
        {
            var exception = await Assert.ThrowsAsync<ProcoreApiException>(
                () => CoreClient.GetCompanyAsync(invalidCompanyId));

            exception.Should().NotBeNull("Should throw ProcoreApiException for invalid company ID");
            exception.StatusCode.Should().Be(404, "Should return 404 for non-existent company");
            
            Output.WriteLine($"Invalid company ID handled correctly: {exception.Message}");
            
            return true;
        });
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Focus", "ErrorHandling")]
    public async Task GetUser_With_Invalid_Id_Should_Handle_Error_Gracefully()
    {
        // Arrange
        const int invalidUserId = 999999999;

        // Act & Assert
        await ExecuteWithTrackingAsync("GetUser_InvalidId", async () =>
        {
            var exception = await Assert.ThrowsAsync<ProcoreApiException>(
                () => CoreClient.GetUserAsync(TestCompanyId, invalidUserId));

            exception.Should().NotBeNull("Should throw ProcoreApiException for invalid user ID");
            exception.StatusCode.Should().Be(404, "Should return 404 for non-existent user");
            
            Output.WriteLine($"Invalid user ID handled correctly: {exception.Message}");
            
            return true;
        });
    }

    #endregion

    #region Performance and Concurrency Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Focus", "Performance")]
    public async Task Concurrent_Company_Requests_Should_Handle_Load()
    {
        // Act
        const int concurrencyLevel = 5;
        var results = await ExecuteConcurrentOperationsAsync(
            "ConcurrentCompanyRequests",
            async index =>
            {
                var companies = await CoreClient.GetCompaniesAsync();
                return companies.Count();
            },
            concurrencyLevel);

        // Assert
        results.Should().HaveCount(concurrencyLevel, "All concurrent requests should succeed");
        results.Should().OnlyContain(count => count > 0, "All requests should return companies");
        
        // All results should be the same
        var expectedCount = results[0];
        results.Should().OnlyContain(count => count == expectedCount, 
            "All concurrent requests should return same number of companies");
        
        Output.WriteLine($"Concurrent requests completed successfully. Company count: {expectedCount}");
        
        ValidatePerformance("ConcurrentCompanyRequests", TestConfig.PerformanceThresholds.ApiOperationMs);
    }

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Focus", "Performance")]
    public async Task Bulk_User_Retrieval_Should_Complete_Within_Time_Limit()
    {
        // Arrange
        var companies = await CoreClient.GetCompaniesAsync();
        var companyIds = companies.Take(3).Select(c => c.Id).ToList();

        // Act
        await AssertCompletesWithinAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var allUsers = new List<User>();
                foreach (var companyId in companyIds)
                {
                    var users = await CoreClient.GetUsersAsync(companyId);
                    allUsers.AddRange(users);
                }
                
                Output.WriteLine($"Retrieved {allUsers.Count} users from {companyIds.Count} companies");
            },
            "BulkUserRetrieval");
    }

    #endregion

    #region Data Consistency Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Focus", "DataConsistency")]
    public async Task User_Data_Should_Be_Consistent_Across_Requests()
    {
        // Arrange
        var currentUser = await CoreClient.GetCurrentUserAsync();
        var companyUsers = await CoreClient.GetUsersAsync(TestCompanyId);
        
        // Act & Assert
        await ExecuteWithTrackingAsync("UserDataConsistency", async () =>
        {
            // Find current user in company user list
            var userInList = companyUsers.FirstOrDefault(u => u.Id == currentUser.Id);
            
            if (userInList != null)
            {
                userInList.Email.Should().Be(currentUser.Email, 
                    "User email should be consistent across different endpoints");
                userInList.FirstName.Should().Be(currentUser.FirstName, 
                    "User first name should be consistent across different endpoints");
                userInList.LastName.Should().Be(currentUser.LastName, 
                    "User last name should be consistent across different endpoints");
                
                Output.WriteLine("User data consistency validated successfully");
            }
            else
            {
                Output.WriteLine("Current user not found in company user list - this may be expected in some sandbox configurations");
            }
            
            return true;
        });
    }

    #endregion

    #region Type Mapping Validation Tests

    [Fact]
    [Trait("Category", "Integration")]
    [Trait("Client", "Core")]
    [Trait("Focus", "TypeMapping")]
    public async Task API_Responses_Should_Map_To_Correct_Types()
    {
        // Act & Assert
        await ExecuteWithTrackingAsync("TypeMappingValidation", async () =>
        {
            // Test Company type mapping
            var companies = await CoreClient.GetCompaniesAsync();
            if (companies.Any())
            {
                var company = companies.First();
                ValidateCompanyTypeMapping(company);
            }

            // Test User type mapping  
            var currentUser = await CoreClient.GetCurrentUserAsync();
            ValidateUserTypeMapping(currentUser);

            // Test Documents type mapping (if any exist)
            var documents = await CoreClient.GetDocumentsAsync(TestCompanyId);
            if (documents.Any())
            {
                var document = documents.First();
                ValidateDocumentTypeMapping(document);
            }

            Output.WriteLine("Type mapping validation completed successfully");
            return true;
        });
    }

    private void ValidateCompanyTypeMapping(Company company)
    {
        company.Should().NotBeNull("Company should not be null");
        company.Id.Should().BeGreaterThan(0, "Company ID should be positive");
        company.Name.Should().NotBeNullOrEmpty("Company name should not be empty");
        company.CreatedAt.Should().NotBe(default(DateTime), "Company created date should be set");
        
        if (!string.IsNullOrEmpty(company.Phone))
        {
            company.Phone.Should().MatchRegex(@"^[\d\s\-\(\)\+\.]+$", "Phone should contain valid characters");
        }
    }

    private void ValidateUserTypeMapping(User user)
    {
        user.Should().NotBeNull("User should not be null");
        user.Id.Should().BeGreaterThan(0, "User ID should be positive");
        user.Email.Should().NotBeNullOrEmpty("User email should not be empty");
        user.Email.Should().Contain("@", "User email should be valid email format");
        user.FirstName.Should().NotBeNullOrEmpty("User first name should not be empty");
        user.LastName.Should().NotBeNullOrEmpty("User last name should not be empty");
        user.CreatedAt.Should().NotBe(default(DateTime), "User created date should be set");
    }

    private void ValidateDocumentTypeMapping(Document document)
    {
        document.Should().NotBeNull("Document should not be null");
        document.Id.Should().BeGreaterThan(0, "Document ID should be positive");
        document.Name.Should().NotBeNullOrEmpty("Document name should not be empty");
        document.CreatedAt.Should().NotBe(default(DateTime), "Document created date should be set");
        
        if (document.FileSize.HasValue)
        {
            document.FileSize.Value.Should().BeGreaterThan(0, "Document file size should be positive");
        }
    }

    #endregion
}

// Placeholder model classes - these would be replaced with actual SDK models
public class Company : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class User : IIdentifiable
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class Document : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long? FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DocumentFolder : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CustomFieldDefinition : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
}

public class ProcoreApiException : Exception
{
    public int StatusCode { get; set; }
    
    public ProcoreApiException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message) { }
}