using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Aki32Utilities.Azure.B2CAuth.FromConsoleViaGlyph;
/// <summary>
/// 
/// Graph を利用して，AAD B2C の情報にアクセス。ユーザーサイドでは実装しない！
/// 参考：
///  前編： https://blog.beachside.dev/entry/2020/04/03/190000
///  後編： https://blog.beachside.dev/entry/2020/04/21/184500
///
/// ※ Microsoft.Graph.Auth が廃止されたため，更新した。
/// 
/// </summary>
class Program
{

    private const string TenantId = "4e79f82f-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
    private const string AppId = "18d1bdaf-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
    private const string ClientSecret = "djv_i-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
    private const string B2CExtensionAppClientId = "a96f5ebd-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

    private const string TenantAttributeName = "TestUserData"; // TODO

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    static async Task Main(string[] args)
    {
        var targetUserObjectId = "ea4c9d71-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // TODO
        var tenantToUpdate = "こんにちは２"; // TODO

        var graphServiceClient = CreateGraphServiceClient();

        WriteLineTitle("\n\n Show User by ObjectID \n\n");
        await ShowUserAsync(graphServiceClient, targetUserObjectId);

        WriteLineTitle("\n\n Show User with Custom attribute \n\n");
        await ShowUserWithCustomAttributeAsync(graphServiceClient, targetUserObjectId, TenantAttributeName);

        WriteLineTitle("\n\n Update User OfficeLocation and Tenant");
        await UpdateUserAsync(graphServiceClient, targetUserObjectId, TenantAttributeName, tenantToUpdate);

        Console.ReadKey();
    }

    private static GraphServiceClient CreateGraphServiceClient()
    {
        var confidentialClientApplication = ConfidentialClientApplicationBuilder
            .Create(AppId)
            .WithTenantId(TenantId)
            .WithClientSecret(ClientSecret)
            .Build();

        // The app registration should be configured to require access to permissions
        // sufficient for the Microsoft Graph API calls the app will be making, and
        // those permissions should be granted by a tenant administrator.
        var scopes = new string[] { "https://graph.microsoft.com/.default" };

        // Build the Microsoft Graph client. As the authentication provider, set an async lambda
        // which uses the MSAL client to obtain an app-only access token to Microsoft Graph,
        // and inserts this access token in the Authorization header of each API request. 
        var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
        {
            // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
            var authResult = await confidentialClientApplication
                .AcquireTokenForClient(scopes)
                .ExecuteAsync();

            // Add the access token in the Authorization header of the API request.
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
        }));

        return graphServiceClient;

    }


    private static async Task ShowUserAsync(GraphServiceClient graphServiceClient, string objectId)
    {
        var user = await graphServiceClient.Users[objectId]
            .Request()
            .Select(u => new { u.DisplayName, u.Id, u.OfficeLocation, u.Identities })
            .GetAsync();

        Console.WriteLine(JsonSerializer.Serialize(user, JsonSerializerOptions));
    }

    private static async Task ShowUserWithCustomAttributeAsync(GraphServiceClient graphServiceClient, string objectId, string attributeName)
    {
        var attributeFullName = GetCustomAttributeFullName(attributeName, B2CExtensionAppClientId);

        var user = await graphServiceClient.Users[objectId]
            .Request()
            .Select($"id, {attributeFullName}") // displayName, OfficeLocation, identities, 
            .GetAsync();

        Console.WriteLine(JsonSerializer.Serialize(user, JsonSerializerOptions));
    }


    private static async Task UpdateUserAsync(GraphServiceClient graphServiceClient, string objectId, string attributeName, string attributeValueToUpdate)
    {
        var attributeFullName = GetCustomAttributeFullName(attributeName, B2CExtensionAppClientId);

        var customAttribute = new Dictionary<string, object>
        {
            [attributeFullName] = attributeValueToUpdate
        };

        var userToUpdate = new User
        {
            OfficeLocation = $"{attributeValueToUpdate}_の隣",
            AdditionalData = customAttribute
        };

        await graphServiceClient.Users[objectId]
            .Request()
            .UpdateAsync(userToUpdate);

        WriteLineTitle(" Updated \n\n");
        await ShowUserWithCustomAttributeAsync(graphServiceClient, objectId, attributeName);
    }

    private static void WriteLineTitle(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }


    private static string GetCustomAttributeFullName(string attributeName, string b2cExtensionAppClientId)
    {
        if (string.IsNullOrWhiteSpace(attributeName))
            throw new ArgumentNullException(nameof(attributeName));

        return $"extension_{b2cExtensionAppClientId.Replace("-", string.Empty)}_{attributeName}";
    }
}
