using Microsoft.Identity.Client;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32Utilities.Azure.B2CAuth.FromApps.LogOn
{
    public class B2CAuthenticationService
    {
        // ★★★★★★★★★★★★★★★ 必要なものたち

        private readonly IPublicClientApplication _pca;

        private static readonly Lazy<B2CAuthenticationService> lazy
            = new Lazy<B2CAuthenticationService>(() => new B2CAuthenticationService());

        public static B2CAuthenticationService Instance { get { return lazy.Value; } }

        private B2CAuthenticationService()
        {

            // default redirectURI; each platform specific project will have to override it with its own
            var builder = PublicClientApplicationBuilder.Create(B2CConstants.ClientID)
                .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
                .WithIosKeychainSecurityGroup(B2CConstants.IOSKeyChainGroup)
                .WithRedirectUri($"msal{B2CConstants.ClientID}://auth");

            // UWP does not require this
            //var windowLocatorService = dependencyservice.get<iparentwindowlocatorservice>();
            //if (windowlocatorservice != null)
            //    builder = builder.withParentActivityOrWindow(() => windowLocatorService?.GetCurrentParentWindow());

            _pca = builder.Build();
        }

        // ★★★★★★★★★★★★★★★ 外部アクセスメソッドたち

        public async Task<UserContext> SignInAsync()
        {
            UserContext newContext = null;
            try
            {
                try
                {
                    // acquire token silent
                    newContext = await AcquireTokenSilent();
                }
                catch (MsalUiRequiredException)
                {
                    // acquire token interactive
                    newContext = await SignInInteractively();
                }
            }
            catch (Exception)
            {
                // user cancelled or many other reasons
            }

            return newContext;
        }

        public async Task<UserContext> SignInSilentAsync()
        {
            UserContext newContext = null;
            try
            {
                // acquire token silent
                newContext = await AcquireTokenSilent();
            }
            catch (Exception)
            {
            }
            return newContext;
        }

        public async Task<UserContext> ResetPasswordAsync()
        {
            AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
                .WithPrompt(Prompt.NoPrompt)
                .WithAuthority(B2CConstants.AuthorityPasswordReset)
                .ExecuteAsync();

            var userContext = UpdateUserInfo(authResult);

            return userContext;
        }

        public async Task<UserContext> EditProfileAsync()
        {
            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();

            AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
                .WithAccount(GetAccountByPolicy(accounts, B2CConstants.PolicyEditProfile))
                .WithPrompt(Prompt.NoPrompt)
                .WithAuthority(B2CConstants.AuthorityEditProfile)
                .ExecuteAsync();

            var userContext = UpdateUserInfo(authResult);

            return userContext;
        }

        public async Task<UserContext> SignOutAsync()
        {

            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();
            while (accounts.Any())
            {
                await _pca.RemoveAsync(accounts.FirstOrDefault());
                accounts = await _pca.GetAccountsAsync();
            }
            var signedOutContext = new UserContext();
            signedOutContext.IsLoggedOn = false;
            return signedOutContext;
        }

        // ★★★★★★★★★★★★★★★ 内部専用メソッドたち

        private async Task<UserContext> AcquireTokenSilent()
        {
            IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();
            AuthenticationResult authResult = await _pca.AcquireTokenSilent(B2CConstants.Scopes, GetAccountByPolicy(accounts, B2CConstants.PolicySignUpSignIn))
               .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
               .ExecuteAsync();

            var newContext = UpdateUserInfo(authResult);
            return newContext;
        }

        private async Task<UserContext> SignInInteractively()
        {
            AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
                .ExecuteAsync();

            var newContext = UpdateUserInfo(authResult);
            return newContext;
        }

        private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
        {
            foreach (var account in accounts)
            {
                string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];
                if (userIdentifier.EndsWith(policy.ToLower()))
                    return account;
            }

            return null;
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        private UserContext UpdateUserInfo(AuthenticationResult ar)
        {
            var newContext = new UserContext();
            newContext.IsLoggedOn = false;
            JObject user = ParseIdToken(ar.IdToken);

            newContext.AccessToken = ar.AccessToken;
            newContext.IdToken = ar.IdToken;
            newContext.Name = user["name"]?.ToString();
            newContext.UserIdentifier = user["sub"]?.ToString();

            newContext.GivenName = user["given_name"]?.ToString();
            newContext.FamilyName = user["family_name"]?.ToString();

            newContext.IsLoggedOn = true;

            return newContext;
        }

        private JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        // ★★★★★★★★★★★★★★★ 内部専用メソッドたち

    }
}
