

namespace Aki32Utilities.Azure.B2CAuth.FromApps.LogOn
{
    public static class B2CConstants
    {
        // solutioinhub.onmicrosoft.com

        // Azure AD B2C Coordinates

        internal static string ApiEndpoint = "http://localhost:7071/api/auth-test/auth";
        //public static string ApiEndpoint = "https://solhub-test2.azurewebsites.net/api/func";
        //public static string ApiEndpoint = "https://solhub-test2.azurewebsites.net/api/hello?name=Taichi";

        internal static string TenantName = "solutioinhub.onmicrosoft.com"; //"app-kobo.com";
        internal static string LoginTenantName = "solutioinhub.b2clogin.com";
        internal static string ClientID = "dc3bbc69-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

        internal static string PolicySignUpSignIn = "b2c_1_susi";
        internal static string PolicyEditProfile = "b2c_1_edit_profile";
        internal static string PolicyResetPassword = "b2c_1_reset";

        internal static string[] Scopes =
        {
            "https://awababy.onmicrosoft.com/0b6603e5-f305-4b8f-9692-7e1b25856c64/user_impersonation",
            "openid",
            "offline_access",
        };

        internal static string AuthorityBase = $"https://{LoginTenantName}/tfp/{TenantName}/";
        internal static string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";
        internal static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
        internal static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";
        internal static string IOSKeyChainGroup = "com.app-kobo.SolHubViewer";

    }
}
