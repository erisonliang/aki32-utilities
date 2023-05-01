using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

namespace Aki32Utilities.Azure.B2CAuth;
/// <summary>
/// 強制的に手動で認証！！
/// </summary>
/// <remarks>
/// 
/// ※NuGet の Microsoft.IdentityModel.Protocols.OpenIdConnect は，当面の間 ver. 5.3.0 を使う！！！
///
/// 参考資料：
/// Functions部分： https://github.com/Azure-Samples/ms-identity-dotnet-webapi-azurefunctions
/// Validate Token部分： https://developer.okta.com/code/dotnet/jwt-validation/#validate-a-token
/// 
/// </remarks>
public static class BootLoader
{
    /// <summary>
    /// tokenを検証します。
    /// </summary>
    /// <param name="req"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    internal static async Task<UserContext?> ValidateToken(HttpRequest req, ILogger log, CancellationToken ct = default)
    {
        try
        {
            IdentityModelEventSource.ShowPII = true;

            // JWT から Access Token を取得。
            var token = GetAccessToken(req);
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            // 署名キー一覧を取得。
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(Constants.OIDCMetadata, new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever());
            var discoveryDocument = await configurationManager.GetConfigurationAsync(ct);
            var signingKeys = discoveryDocument.SigningKeys;

            // パラメーター
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = Constants.ClientID,

                RequireExpirationTime = false,
                RequireSignedTokens = false,

                ValidateIssuer = true,
                ValidIssuer = Constants.Issuer,

                ValidateIssuerSigningKey = false,
                IssuerSigningKeys = signingKeys,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),  // 2分以下が良い。

            };

            // 一致の認証
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParameters, out var rawValidatedToken);

            return GetUserInfo((JwtSecurityToken)rawValidatedToken);
        }
        catch (SecurityTokenValidationException ex)
        {
            log.LogWarning($"SecurityTokenValidation Exception: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            log.LogWarning($"Exception: {ex.Message}");
            return null;
        }
    }

    private static string? GetAccessToken(HttpRequest req)
    {
        var authorizationHeader = req.Headers?["Authorization"];
        string[] parts = authorizationHeader?.ToString().Split(null) ?? new string[0];
        if (parts.Length == 2 && parts[0].Equals("Bearer"))
            return parts[1];
        return null;
    }

    private static UserContext GetUserInfo(JwtSecurityToken token)
    {
        var newContext = new UserContext
        {
            IsLoggedOn = false
        };

        var user = token.Payload;

        newContext.AccessToken = token.RawData;

        newContext.Name = user["name"]?.ToString()!;
        newContext.UserIdentifier = user["sub"]?.ToString()!;

        newContext.GivenName = user["given_name"]?.ToString()!;
        newContext.FamilyName = user["family_name"]?.ToString()!;

        if (user["emails"] is JArray emails)
            newContext.Email = emails[0].ToString();

        newContext.IsLoggedOn = true;

        return newContext;
    }

}