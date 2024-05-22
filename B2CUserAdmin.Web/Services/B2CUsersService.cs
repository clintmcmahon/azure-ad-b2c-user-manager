using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Reflection;
namespace B2CUserAdmin.Web.Services;

public class B2CUsersService
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly IConfiguration _configuration;

    public B2CUsersService(IConfiguration configuration)
    {
        // The client credentials flow requires that you request the
        // /.default scope, and pre-configure your permissions on the
        // app registration in Azure. An administrator must grant consent
        // to those permissions beforehand.
        var scopes = new[] { "https://graph.microsoft.com/.default" };

        // using Azure.Identity;
        var options = new ClientSecretCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
        };

        var tenantId = configuration["AzureAdB2C:TenantId"];
        var clientId = configuration["AzureAdB2C:ClientId"];
        var clientSecret = configuration["AzureAdB2C:ClientSecret"];

        // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, options);

        _graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
        _configuration = configuration;

    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await _graphServiceClient.Users
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = ["displayName", "id", "identities", "otherMails"];

            });

        return users.Value;
    }

    public async Task<User> GetUserAsync(string userId)
    {
        return await _graphServiceClient.Users[userId.ToString()]
            .GetAsync();
    }

    public async Task<User> GetUserAllProperties(string userId)
    {
        var b2cAppId = await GetB2CExtensionsAppIdAsync();
        var customAttributes = await GetCustomExtensionAttributes();
        var baseProperties = new[]
        {
            "identities", "displayName", "givenName", "surname", "userPrincipalName", "accountEnabled",
            "ageGroup", "consentProvidedForMinor", "country", "creationType", "department",
            "employeeId", "faxNumber", "legalAgeGroupClassification", "mail",
            "mobilePhone", "officeLocation", "otherMails", "passwordPolicies", "postalCode",
            "preferredDataLocation", "preferredLanguage", "proxyAddresses", "showInAddressList",
            "state", "streetAddress", "city", "zipCode", "usageLocation", "id", "userType", "jobTitle", "companyName", "employeeType","businessPhone","mobilePhone"
        };

        var customAttributesArray = customAttributes.Select(attr => attr.Id).ToArray();
        var allProperties = baseProperties.Concat(customAttributesArray).ToArray();

        // Make a call to Microsoft Graph to get the user with the specified properties.
        var user = await _graphServiceClient.Users[userId.ToString()]
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Select = allProperties;

            });

        return user;
    }

    public async Task UpdateUserAsync(string userId, User updatedUser)
    {
        await _graphServiceClient.Users[userId.ToString()]
            .PatchAsync(updatedUser);
    }

    public async Task<List<IdentityUserFlowAttribute>> GetCustomExtensionAttributes()
    {
        var extensionProperties = await _graphServiceClient.Identity.UserFlowAttributes.GetAsync();
        return extensionProperties.Value.Where(x => x.Id.StartsWith("extension_")).ToList();
    }

    public async Task<string> GetB2CExtensionsAppIdAsync()
    {
        try
        {
            var apps = await _graphServiceClient.Applications.GetAsync(requestConfiguration =>
            {
                //requestConfiguration.QueryParameters.Select = new[] { "id", };
                requestConfiguration.QueryParameters.Filter = "startswith(displayName, 'b2c-extensions-app')";
            });

            return apps.Value.FirstOrDefault().AppId;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string GetVersion()
    {
        var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        return version;
    }

}
