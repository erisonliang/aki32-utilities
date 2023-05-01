

namespace Aki32Utilities.Azure.B2CAuth;
/// <summary>
/// demo code, usually want to pull these from key vault or config etc.
/// </summary>
internal static class Constants
{
    // ★★★★★ ポリシー系

    internal static string PolicySignUpSignIn = "b2c_1_susi";
    internal static string PolicyEditProfile = "b2c_1_edit_profile";

    // ★★★★★ サーバー系

    internal static string ClientID = "dc3bbc69-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
    internal static string TenantName = "xxxxxx.onmicrosoft.com";
    internal static string LoginTenantName = "xxxxxx.b2clogin.com";
    internal static string TenantId = "4e79f82f-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

    internal static string Issuer = $"https://{LoginTenantName}/{TenantId}/v2.0/";
    internal static string OIDCMetadata = $"https://{LoginTenantName}/{TenantName}/{PolicySignUpSignIn}/v2.0/.well-known/openid-configuration";

    // ★★★★★
}