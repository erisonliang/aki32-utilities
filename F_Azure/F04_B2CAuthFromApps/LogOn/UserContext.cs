

namespace Aki32Utilities.Azure.B2CAuth.FromApps.LogOn
{
    public class UserContext
    {
        public string UserIdentifier { get; internal set; }

        public string Name { get; internal set; }
        public string GivenName { get; internal set; }
        public string FamilyName { get; internal set; }

        public bool IsLoggedOn { get; internal set; }
        public string AccessToken { get; internal set; }
        public string IdToken { get; internal set; }
    }
}
